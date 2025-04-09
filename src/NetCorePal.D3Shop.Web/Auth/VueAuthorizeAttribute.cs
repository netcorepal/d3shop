using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using NetCorePal.D3Shop.Admin.Shared.Authorization;

namespace NetCorePal.D3Shop.Web.Auth
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class VueAuthorizeAttribute : AuthorizeAttribute, IAuthorizationRequirementData
    {
        private readonly string[] _permissionCodes;

        public VueAuthorizeAttribute(params string[] permissionCodes)
        {
            _permissionCodes = permissionCodes;
            // 设置认证方案为 JWT Bearer 认证
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }

        public IEnumerable<IAuthorizationRequirement> GetRequirements()
        {
            return _permissionCodes.Select(code => new PermissionRequirement(code));
        }
    }
}
