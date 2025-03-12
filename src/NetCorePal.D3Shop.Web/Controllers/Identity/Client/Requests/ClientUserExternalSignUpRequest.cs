namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client.Requests;

public record ClientUserExternalSignUpRequest(
    string SignupToken,
    string Phone,
    string Password);