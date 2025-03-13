using System.Security.Claims;
using NetCorePal.D3Shop.Admin.Shared.Authorization;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;

namespace NetCorePal.D3Shop.Web.Auth;

public class ServerPermissionChecker(AdminUserQuery adminUserQuery)
    : IPermissionChecker
{
    public async Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permissionCode)
    {
        if (string.IsNullOrWhiteSpace(permissionCode))
            throw new ArgumentException(@"Permission code cannot be null or empty", nameof(permissionCode));

        var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString))
            throw new InvalidOperationException("User Id is missing in ClaimsPrincipal.");

        if (!long.TryParse(userIdString, out var userId))
            throw new InvalidOperationException("User Id could not be parsed to a valid long value.");

        var adminUserPermissions = await adminUserQuery.GetAdminUserPermissionCodes(new AdminUserId(userId));

        return adminUserPermissions?.Contains(permissionCode) ?? false;
    }
}