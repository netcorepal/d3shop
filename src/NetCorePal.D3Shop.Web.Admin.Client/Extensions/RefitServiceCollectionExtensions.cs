using System.Reflection;
using NetCorePal.D3Shop.Web.Admin.Client.Attributes;

namespace NetCorePal.D3Shop.Web.Admin.Client.Extensions;

public static class RefitServiceCollectionExtensions
{
    /// <summary>
    /// 批量注册标记了 <see cref="RefitServiceAttribute"/> 特性的 Refit 服务接口。
    /// </summary>
    /// <param name="services"></param>
    /// <param name="baseUrl"></param>
    public static void AddRefitServices(this IServiceCollection services, string baseUrl)
    {
        // 获取所有标记了 RefitServiceAttribute 特性的接口
        var serviceTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsInterface && t.GetCustomAttribute<RefitServiceAttribute>() != null);

        var ser = new NewtonsoftJsonContentSerializer();
        var settings = new RefitSettings(ser);
        foreach (var serviceType in serviceTypes)
        {
            services.AddRefitClient(serviceType, settings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));
        }
    }
}