using System.Reflection;
using AntDesign;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetCorePal.D3Shop.Admin.Shared.Authorization;
using NetCorePal.D3Shop.Web.Admin.Client.Auth;
using NetCorePal.D3Shop.Web.Application.Hubs;
using NetCorePal.D3Shop.Web.Application.IntegrationEventHandlers;
using NetCorePal.D3Shop.Web.Auth;
using NetCorePal.D3Shop.Web.Blazor;
using NetCorePal.D3Shop.Web.Blazor.Components;
using NetCorePal.D3Shop.Web.Clients;
using NetCorePal.D3Shop.Web.Extensions;
using NetCorePal.D3Shop.Web.Helper;
using NetCorePal.Extensions.Domain.Json;
using NetCorePal.Extensions.MultiEnv;
using NetCorePal.Extensions.NewtonsoftJson;
using NetCorePal.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Prometheus;
using Refit;
using Serilog;
using StackExchange.Redis;
using _Imports = NetCorePal.D3Shop.Web.Admin.Client._Imports;

Log.Logger = new LoggerConfiguration()
    .Enrich.WithClientIp()
    .WriteTo.Console( /*new JsonFormatter()*/)
    .CreateLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    #region SignalR

    builder.Services.AddHealthChecks();
    builder.Services.AddMvc().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new NewtonsoftEntityIdJsonConverter());
    });
    builder.Services.AddSignalR();

    #endregion

    #region Prometheus监控

    builder.Services.AddHealthChecks().ForwardToPrometheus();
    builder.Services.AddHttpClient(Options.DefaultName)
        .UseHttpClientMetrics();

    #endregion

    // Add services to the container.

    #region 身份认证

    var redis = await ConnectionMultiplexer.ConnectAsync(builder.Configuration.GetConnectionString("Redis")!);
    builder.Services.AddSingleton<IConnectionMultiplexer>(_ => redis);
    builder.Services.AddDataProtection()
        .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

    builder.Services.GetApplicationSettings(builder.Configuration);
    builder.Services.AddAuthenticationSchemes();
    builder.Services.AddNetCorePalJwt().AddRedisStore();
    builder.Services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
    builder.Services.AddTransient<IPermissionChecker, ServerPermissionChecker>();

    builder.Services.AddSingleton<TokenGenerator>();

    builder.Services.AddScoped<ICurrentClientUser, CurrentClientUser>();

    #endregion

    #region Controller

    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new EntityIdJsonConverterFactory());
    });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddEntityIdSchemaMap(); //强类型id swagger schema 映射
        c.AddJwtSecurity(); //添加jwt认证
    });

    #endregion

    #region 公共服务

    builder.Services.AddSingleton<IClock, SystemClock>();

    #endregion

    #region 集成事件

    builder.Services.AddTransient<OrderPaidIntegrationEventHandler>();

    #endregion

    #region 模型验证器

    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    builder.Services.AddKnownExceptionErrorModelInterceptor();

    #endregion

    #region Mapper Provider

    builder.Services.AddMapperPrivider(Assembly.GetExecutingAssembly());

    #endregion

    #region Query

    builder.Services.AddAllQueries(Assembly.GetExecutingAssembly());

    #endregion


    #region 基础设施

    builder.Services.AddRepositories(typeof(ApplicationDbContext).Assembly);

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseMySql(builder.Configuration.GetConnectionString("MySql"),
            new MySqlServerVersion(new Version(8, 0, 34)),
            b =>
            {
                b.MigrationsAssembly(typeof(Program).Assembly.FullName);
                b.TranslateParameterizedCollectionsToConstants();
            });
        options.LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    });
    builder.Services.AddUnitOfWork<ApplicationDbContext>();
    builder.Services.AddMySqlTransactionHandler();
    builder.Services.AddRedisLocks();
    //配置多环境Options
    builder.Services.Configure<EnvOptions>(envOptions => builder.Configuration.GetSection("Env").Bind(envOptions));
    builder.Services.AddContext().AddEnvContext().AddCapContextProcessor();
    builder.Services.AddNetCorePalServiceDiscoveryClient();
    builder.Services.AddIntegrationEventServices(typeof(Program))
        .AddIIntegrationEventConverter(typeof(Program))
        .UseCap(typeof(Program))
        .AddContextIntegrationFilters();
    builder.Services.AddCap(x =>
    {
        x.UseEntityFramework<ApplicationDbContext>();
        x.UseRabbitMQ(p => builder.Configuration.GetSection("RabbitMQ").Bind(p));
        x.UseDashboard(); //CAP Dashboard  path：  /cap
    });

    #endregion

    builder.Services.AddMemoryCache();

    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly())
            .AddKnownExceptionValidationBehavior()
            .AddUnitOfWorkBehaviors());

    #region 远程服务客户端配置

    var ser = new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    });
    var settings = new RefitSettings(ser);
    builder.Services.AddRefitClient<IUserServiceClient>(settings)
        .ConfigureHttpClient(client =>
            client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UserService:BaseUrl")!))
        .AddStandardResilienceHandler(); //添加标准的重试策略

    #endregion

    #region Jobs

    builder.Services.AddHangfire(x => { x.UseRedisStorage(builder.Configuration.GetConnectionString("Redis")); });
    builder.Services.AddHangfireServer(); //hangfire dashboard  path：  /hangfire

    #endregion

    builder.Services.AddHttpContextAccessor();

    #region Blazor

    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents()
        .AddInteractiveWebAssemblyComponents();

    builder.Services.AddAntDesign();
    // 设置默认语言
    LocaleProvider.SetLocale("zh-CN");

    builder.Services.AddCascadingAuthenticationState();
    builder.Services.AddSingleton<IAuthorizationPolicyProvider, ClientPermissionPolicyProvider>();
    builder.Services.AddScoped<AuthenticationStateProvider, PersistingServerAuthenticationStateProvider>();

    builder.Services.AddClientServices();

    #endregion

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        app.SeedDatabase();
    }

    app.UseKnownExceptionHandler();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseAuthorization();
    app.UseAntiforgery();
    app.MapControllers();

    #region SignalR

    app.MapHub<ChatHub>("/chat");

    #endregion

    app.UseHttpMetrics();
    app.MapHealthChecks("/health");
    app.MapMetrics(); // 通过   /metrics  访问指标
    app.UseHangfireDashboard();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode()
        .AddInteractiveWebAssemblyRenderMode()
        .AddAdditionalAssemblies(typeof(_Imports).Assembly)
        .AllowAnonymous();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}

#pragma warning disable S1118
public partial class Program
#pragma warning restore S1118
{
}