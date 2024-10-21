namespace NetCorePal.D3Shop.Web.Controllers.Identity.Requests;

public record UpdateAdminUserPasswordRequest(string OldPassword, string NewPassword);