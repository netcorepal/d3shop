namespace NetCorePal.D3Shop.Admin.Shared.Requests;

public record CreateRoleRequest(string Name, string Description, IEnumerable<string> PermissionCodes);