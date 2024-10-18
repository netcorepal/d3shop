namespace NetCorePal.D3Shop.Web.Controllers.Identity.Responses;

public record RolePermissionResponse(string PermissionCode, string Remark, string GroupName, bool IsAssigned);