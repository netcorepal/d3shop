namespace NetCorePal.D3Shop.Web.Controllers.Identity.Requests;

public record CreateRoleRequest(string Name, string Description, IEnumerable<string> PermissionCodes);