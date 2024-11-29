namespace NetCorePal.D3Shop.Admin.Shared.Responses;

public record AdminUserPermissionResponse(
    string Code,
    string GroupName,
    string Remark,
    bool IsAssigned,
    bool IsFromRole);