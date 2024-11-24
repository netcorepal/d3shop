using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Admin.Client.Services;
using NetCorePal.D3Shop.Web.Controllers.Identity;
using NetCorePal.Extensions.Dto;

namespace NetCorePal.D3Shop.Web.Blazor.Services;

public class AdminUserService(AdminUserController adminUserController) : IAdminUserService
{
    public Task<ResponseData<AdminUserId>> CreateAdminUser(CreateAdminUserRequest request)
    {
        return adminUserController.CreateAdminUser(request);
    }

    public Task<ResponseData<List<AdminUserResponse>>> GetAllAdminUsers(AdminUserQueryRequest request)
    {
        return adminUserController.GetAllAdminUsers(request);
    }

    public Task<ResponseData<List<RoleId>>> GetAssignedRoleIds(AdminUserId id)
    {
        return adminUserController.GetAssignedRoleIds(id);
    }

    public Task<ResponseData> UpdateAdminUserRoles(AdminUserId id, IEnumerable<RoleId> roleIds)
    {
        return adminUserController.UpdateAdminUserRoles(id, roleIds);
    }

    public Task<ResponseData> DeleteAdminUser(AdminUserId id)
    {
        return adminUserController.DeleteAdminUser(id);
    }

    public Task<ResponseData<List<RoleNameResponse>>> GetAllRoleNames()
    {
        return adminUserController.GetAllRoleNames();
    }

    public Task<ResponseData<List<AdminUserPermissionResponse>>> GetAssignedPermissions(AdminUserId id)
    {
        return adminUserController.GetAssignedPermissions(id);
    }

    public Task<ResponseData> SetAdminUserSpecificPermissions(AdminUserId id, IEnumerable<string> permissionCodes)
    {
        return adminUserController.SetAdminUserSpecificPermissions(id, permissionCodes);
    }
}