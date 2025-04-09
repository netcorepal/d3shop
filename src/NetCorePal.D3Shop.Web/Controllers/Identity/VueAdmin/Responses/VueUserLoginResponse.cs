using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Controllers.Identity.Client.Responses;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.VueAdmin.Responses
{
    public class VueUserLoginResponse
    {


        public AdminUserId UserId { get; set; } = default!;

        public string UserName { get; set; } = string.Empty;
        public string RealName { get; set; } = string.Empty;
        public virtual ICollection<RoleId> Roles { get; set; } = [];

        public string HomePath { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string FailedMessage { get; set; } = string.Empty;

        public static VueUserLoginResponse Success(string accessToken, string refreshToken, AdminUserId userId, string userName, string realName, ICollection<RoleId> roles, string homePath)
        {
            return new VueUserLoginResponse { AccessToken = accessToken, RefreshToken = refreshToken, UserName = userName, UserId = userId, RealName = realName, Roles = roles, HomePath = homePath };
        }

        public static VueUserLoginResponse Failure(string message)
        {
            return new VueUserLoginResponse { FailedMessage = message };
        }
    }
}
