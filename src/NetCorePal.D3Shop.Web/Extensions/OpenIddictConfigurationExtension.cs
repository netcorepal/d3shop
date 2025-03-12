using OpenIddict.Client;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace NetCorePal.D3Shop.Web.Extensions;

public static class OpenIddictConfigurationExtension
{
    internal static IServiceCollection ConfigOpenIddict(this IServiceCollection services)
    {
        services.AddOpenIddict()
            // 注册 OpenIddict 核心组件。
            .AddCore(options =>
            {
                // 配置 OpenIddict 使用 Entity Framework Core 存储和模型。
                // 注意：调用 ReplaceDefaultEntities() 可以替换默认的 OpenIddict 实体。
                options.UseEntityFrameworkCore()
                    .UseDbContext<ApplicationDbContext>();

                // 启用 Quartz.NET 集成。
                options.UseQuartz();
            })

            // 注册 OpenIddict 客户端组件。
            .AddClient(options =>
            {
                // 注意：此示例使用授权码流程，但可以根据需要启用其他流程。
                options.AllowAuthorizationCodeFlow();

                // 注册用于保护敏感数据（如 OpenIddict 生成的状态令牌）的签名和加密凭据。
                options.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();

                // 注册 ASP.NET Core 主机并配置 ASP.NET Core 特定的选项。
                options.UseAspNetCore()
                    .EnableStatusCodePagesIntegration()
                    .EnableRedirectionEndpointPassthrough()
                    .EnablePostLogoutRedirectionEndpointPassthrough();

                // 注册 System.Net.Http 集成，并使用当前程序集的身份作为更具体的用户代理，
                // 这在处理使用用户代理作为请求限流手段的提供者（如 Reddit）时非常有用。
                options.UseSystemNetHttp()
                    .SetProductInformation(typeof(Program).Assembly);

                // 添加与服务器项目中定义的客户端应用程序匹配的客户端注册。
                options.AddRegistration(new OpenIddictClientRegistration
                {
                    ProviderName = "Local",
                    Issuer = new Uri("https://localhost:44313/", UriKind.Absolute),

                    ClientId = "mvc",
                    ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
                    Scopes =
                    {
                        Scopes.Email, Scopes.Profile
                    },

                    // 注意：为了缓解混淆攻击，建议为每个提供者使用唯一的重定向端点 URI，
                    // 除非所有注册的提供者都支持在授权响应中返回包含其 URL 的特殊 "iss" 参数。
                    // 更多信息请参阅：https://datatracker.ietf.org/doc/html/draft-ietf-oauth-security-topics#section-4.4。
                    RedirectUri = new Uri("api/ClientUserAccount/callback/login/local", UriKind.Relative),
                    PostLogoutRedirectUri = new Uri("api/ClientUserAccount/callback/logout/local", UriKind.Relative)
                });

                // 使用 Web 提供者，并添加 GitHub 提供者配置。
                options.UseWebProviders()
                    .AddGitHub(gitHub =>
                    {
                        gitHub.SetClientId("Ov23liUNuxlCfTtN3nKL")
                            .SetClientSecret("a921237878e9adc93c7a8b24502c14f67e23f0a1")
                            .SetRedirectUri("api/ClientUserAccount/callback/login/github");
                    });
            });

        return services;
    }
}