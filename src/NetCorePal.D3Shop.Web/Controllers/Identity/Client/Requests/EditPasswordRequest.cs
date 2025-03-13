namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client.Requests;

public record EditPasswordRequest(
    string OldPassword,
    string NewPassword);