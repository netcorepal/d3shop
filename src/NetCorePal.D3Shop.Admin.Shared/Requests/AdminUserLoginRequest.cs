namespace NetCorePal.D3Shop.Admin.Shared.Requests;

public record AdminUserLoginRequest(string Name, string Password, bool IsPersistent);