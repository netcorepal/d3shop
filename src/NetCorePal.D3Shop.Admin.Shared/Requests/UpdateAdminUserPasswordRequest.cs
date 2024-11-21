namespace NetCorePal.D3Shop.Admin.Shared.Requests;

public record UpdateAdminUserPasswordRequest(string OldPassword, string NewPassword);