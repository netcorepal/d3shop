namespace NetCorePal.D3Shop.Admin.Shared.Responses;

public record RolePermissionResponse(string PermissionCode, string Remark, string GroupName, bool IsAssigned);