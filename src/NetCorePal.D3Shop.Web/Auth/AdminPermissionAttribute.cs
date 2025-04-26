using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using NetCorePal.D3Shop.Admin.Shared.Authorization;

namespace NetCorePal.D3Shop.Web.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AdminPermissionAttribute : AuthorizeAttribute, IAuthorizationRequirementData
    {
        private readonly string[] _permissionCodes;

        public AdminPermissionAttribute(params string[] permissionCodes)
        {
            _permissionCodes = permissionCodes;
            // 同时支持 Cookie 和 JWT 认证
            AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme},Cookies";
        }

        public IEnumerable<IAuthorizationRequirement> GetRequirements()
        {
            return _permissionCodes.Select(code => new PermissionRequirement(code));
        }
    }
}