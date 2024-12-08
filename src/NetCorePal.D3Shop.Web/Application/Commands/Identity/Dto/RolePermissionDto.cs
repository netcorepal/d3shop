namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Dto;

public class RolePermissionDto(string permissionCode, string permissionRemark)
{
    public string PermissionCode { get; } = permissionCode;
    public string PermissionRemark { get; } = permissionRemark;
}