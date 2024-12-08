using AntDesign.ProLayout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NetCorePal.D3Shop.Admin.Shared.Authorization;
using NetCorePal.D3Shop.Web.Admin.Client.Auth;
using NetCorePal.D3Shop.Web.Admin.Client.Extensions;

namespace NetCorePal.D3Shop.Web.Admin.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // Services注册
            builder.Services.AddRefitServices(builder.HostEnvironment.BaseAddress);

            #region 身份认证和授权

            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
            builder.Services.AddSingleton<IAuthorizationPolicyProvider, ClientPermissionPolicyProvider>();
            builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            builder.Services.AddSingleton<IPermissionChecker, ClientPermissionChecker>();

            #endregion

            builder.Services.AddAntDesign();
            // 设置默认语言
            LocaleProvider.SetLocale("zh-CN");

            builder.Services.Configure<ProSettings>(builder.Configuration.GetSection("ProSettings"));

            await builder.Build().RunAsync();
        }
    }
}