using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Admin.Client.Attributes;

namespace NetCorePal.D3Shop.Web.Admin.Client.Services;

[RefitService]
public interface IAdminUserService
{
    [Post("/api/AdminUser/CreateAdminUser")]
    Task<ResponseData<AdminUserId>> CreateAdminUser([Body] CreateAdminUserRequest request);

    [Get("/api/AdminUser/GetAllAdminUsers")]
    Task<ResponseData<PagedData<AdminUserResponse>>> GetAllAdminUsers([Query] AdminUserQueryRequest request);

    [Get("/api/AdminUser/GetAdminUserRoles/{id}")]
    Task<ResponseData<List<AdminUserRoleResponse>>> GetAdminUserRoles(AdminUserId id);

    [Put("/api/AdminUser/UpdateAdminUserRoles/{id}")]
    Task<ResponseData> UpdateAdminUserRoles(AdminUserId id, [Body] IEnumerable<RoleId> roleIds);

    [Delete("/api/AdminUser/DeleteAdminUser/{id}")]
    Task<ResponseData> DeleteAdminUser(AdminUserId id);

    [Get("/api/AdminUser/GetAllRolesForCreateUser")]
    Task<ResponseData<List<AdminUserRoleResponse>>> GetAllRolesForCreateUser();

    [Get("/api/AdminUser/GetAdminUserPermissions/{id}")]
    Task<ResponseData<List<AdminUserPermissionResponse>>> GetAdminUserPermissions(AdminUserId id);

    [Put("/api/AdminUser/SetAdminUserSpecificPermissions/{id}")]
    Task<ResponseData> SetAdminUserSpecificPermissions(AdminUserId id, [Body] IEnumerable<string> permissionCodes);
}