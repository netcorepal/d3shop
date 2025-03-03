using Microsoft.AspNetCore.Authorization;

namespace NetCorePal.D3Shop.Web.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ClientAuthorizeAttribute : AuthorizeAttribute
{
    public ClientAuthorizeAttribute()
    {
        AuthenticationSchemes = "Bearer";
    }
}