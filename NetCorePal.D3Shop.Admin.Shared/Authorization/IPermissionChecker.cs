using System.Security.Claims;

namespace NetCorePal.D3Shop.Admin.Shared.Authorization;

public interface IPermissionChecker
{
    Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permissionCode);
}