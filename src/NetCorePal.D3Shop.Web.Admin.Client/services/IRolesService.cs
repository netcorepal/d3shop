using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Admin.Client.Attributes;

namespace NetCorePal.D3Shop.Web.Admin.Client.Services;

[RefitService]
public interface IRolesService
{
    [Post("/api/Role/CreateRole")]
    Task<ResponseData<RoleId>> CreateRole([Body] CreateRoleRequest request);

    [Get("/api/Role/GetAllRoles")]
    Task<ResponseData<List<RoleResponse>>> GetAllRoles([Query] RoleQueryRequest request);

    [Put("/api/Role/UpdateRoleInfo/{id}")]
    Task<ResponseData> UpdateRoleInfo(RoleId id, [Body] UpdateRoleInfoRequest request);

    [Put("/api/Role/UpdateRolePermissions/{id}")]
    Task<ResponseData> UpdateRolePermissions(RoleId id, [Body] List<string> permissionCodes);

    [Delete("/api/Role/DeleteRole/{id}")]
    Task<ResponseData> DeleteRole(RoleId id);

    [Get("/api/Role/GetRolePermissions/{id}")]
    Task<ResponseData<List<string>>> GetRolePermissions(RoleId id);
}