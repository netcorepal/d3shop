using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Admin.Client.Attributes;

namespace NetCorePal.D3Shop.Web.Admin.Client.Services;

[RefitService]
public interface IRolesService
{
    [Post("/api/Role/CreateRole")]
    Task<ResponseData<RoleId>> CreateRole([Body] CreateRoleRequest request);

    [Get("/api/Role/GetAllRoles")]
    Task<ResponseData<PagedData<RoleResponse>>> GetAllRoles([Query] RoleQueryRequest request);

    [Put("/api/Role/UpdateRoleInfo/{id}")]
    Task<ResponseData> UpdateRoleInfo(RoleId id, [Body] UpdateRoleInfoRequest request);

    [Put("/api/Role/UpdateRolePermissions/{id}")]
    Task<ResponseData> UpdateRolePermissions(RoleId id, [Body] IEnumerable<string> permissionCodes);

    [Delete("/api/Role/DeleteRole/{id}")]
    Task<ResponseData> DeleteRole(RoleId id);

    [Get("/api/Role/GetAssignedPermissionCodes/{id}")]
    Task<ResponseData<List<string>>> GetAssignedPermissionCodes(RoleId id);
}