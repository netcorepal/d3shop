using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.D3Shop.Admin.Shared.Responses
{
    public class AdminUserLoginResponse
    {

        public AdminUserId UserId { get; set; } = default!;

        public string UserName { get; set; } = string.Empty;
        public string RealName { get; set; } = string.Empty;
        public virtual ICollection<RoleId> Roles { get; set; } = [];

        public string HomePath { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string FailedMessage { get; set; } = string.Empty;

        public static AdminUserLoginResponse Success(string accessToken, string refreshToken, AdminUserId userId, string userName, string realName, ICollection<RoleId> roles, string homePath)
        {
            return new AdminUserLoginResponse { AccessToken = accessToken, RefreshToken = refreshToken, UserName = userName, UserId = userId, RealName = realName, Roles = roles, HomePath = homePath };
        }

        public static AdminUserLoginResponse Failure(string message)
        {
            return new AdminUserLoginResponse { FailedMessage = message };
        }
    }
}
