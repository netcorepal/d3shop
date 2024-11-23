using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Admin.Client.Services;
using NetCorePal.D3Shop.Web.Controllers.Identity;
using NetCorePal.Extensions.Dto;

namespace NetCorePal.D3Shop.Web.Blazor.Services;

public class RolesService(RoleController roleController) : IRolesService
{
    public Task<ResponseData<RoleId>> CreateRole(CreateRoleRequest request)
    {
        return roleController.CreateRole(request);
    }

    public Task<ResponseData<List<RoleResponse>>> GetAllRoles(RoleQueryRequest request)
    {
        return roleController.GetAllRoles(request);
    }

    public Task<ResponseData> UpdateRoleInfo(RoleId id, UpdateRoleInfoRequest request)
    {
        return roleController.UpdateRoleInfo(id, request);
    }

    public Task<ResponseData> UpdateRolePermissions(RoleId id, List<string> permissionCodes)
    {
        return roleController.UpdateRolePermissions(id, permissionCodes);
    }

    public Task<ResponseData> DeleteRole(RoleId id)
    {
        return roleController.DeleteRole(id);
    }

    public Task<ResponseData<List<string>>> GetRolePermissions(RoleId id)
    {
        return roleController.GetRolePermissions(id);
    }
}