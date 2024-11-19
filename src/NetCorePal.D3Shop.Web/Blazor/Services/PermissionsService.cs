using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission;
using NetCorePal.D3Shop.Web.Admin.Client.Services;
using NetCorePal.Extensions.Dto;

namespace NetCorePal.D3Shop.Web.Blazor.Services;

public class PermissionsService : IPermissionsService
{
    public Task<ResponseData<IEnumerable<Permission>>> GetAll()
    {
        IEnumerable<Permission> permissions = Permissions.AllPermissions;
        return Task.FromResult(permissions.AsResponseData());
    }
}