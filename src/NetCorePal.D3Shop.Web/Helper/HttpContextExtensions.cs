using System.Net;

namespace NetCorePal.D3Shop.Web.Helper;

public static class HttpContextExtensions
{
    public static IPAddress? GetRemoteIpAddress(this HttpContext context)
    {
        // 处理反向代理场景
        return context.Request.Headers["X-Forwarded-For"].FirstOrDefault() is { } forwardedIp
            ? IPAddress.Parse(forwardedIp)
            : context.Connection.RemoteIpAddress;
    }
}