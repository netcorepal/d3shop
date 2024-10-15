using NetCorePal.Extensions.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Prometheus;
using System.Reflection;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using FluentValidation.AspNetCore;
using FluentValidation;
using NetCorePal.Extensions.Domain.Json;
using NetCorePal.D3Shop.Web.Application.Queries;
using NetCorePal.D3Shop.Web.Application.IntegrationEventHandlers;
using NetCorePal.D3Shop.Web.Clients;
using NetCorePal.D3Shop.Web.Extensions;
using Serilog;
using Serilog.Formatting.Json;
using Hangfire;
using Hangfire.Redis.StackExchange;
using NetCorePal.D3Shop.Web.Application.Queries.Identity;
using NetCorePal.Extensions.AspNetCore.Json;
using NetCorePal.Extensions.MultiEnv;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;

Log.Logger = new LoggerConfiguration()
    .Enrich.WithClientIp()
    .WriteTo.Console(new JsonFormatter())
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

    var redis = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!);
    builder.Services.AddSingleton<IConnectionMultiplexer>(_ => redis);
    builder.Services.AddDataProtection()
        .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

    builder.Services.AddJwtAuthentication(builder.Services.GetApplicationSettings(builder.Configuration));
    builder.Services.AddPermissionAuthorizationServices();
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
        c.AddJwtSecurity();//添加jwt认证
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

    builder.Services.AddScoped<OrderQuery>();
    builder.Services.AddScoped<UserQuery>();
    #endregion



    #region 基础设施
    builder.Services.AddRepositories(typeof(ApplicationDbContext).Assembly);

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseMySql(builder.Configuration.GetConnectionString("MySql"),
            new MySqlServerVersion(new Version(8, 0, 34)),
            b => b.MigrationsAssembly(typeof(Program).Assembly.FullName));
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
        .AddContextIntegrationFilters()
        .AddEnvIntegrationFilters();
    builder.Services.AddCap(x =>
    {
        x.UseEntityFramework<ApplicationDbContext>();
        x.UseRabbitMQ(p => builder.Configuration.GetSection("RabbitMQ").Bind(p));
        x.UseDashboard(); //CAP Dashboard  path：  /cap
    });

    #endregion

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

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.EnsureCreated();
        app.SeedDatabase();
    }

    app.UseKnownExceptionHandler();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseStaticFiles();
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthorization();

    app.MapControllers();

    #region SignalR

    app.MapHub<NetCorePal.D3Shop.Web.Application.Hubs.ChatHub>("/chat");

    #endregion

    app.UseHttpMetrics();
    app.MapHealthChecks("/health");
    app.MapMetrics("/metrics"); // 通过   /metrics  访问指标
    app.UseHangfireDashboard();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

#pragma warning disable S1118
public partial class Program
#pragma warning restore S1118
{
}