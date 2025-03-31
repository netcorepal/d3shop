namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client.Responses;

public record ClientUserExternalSignUpResponse(string Token, string RefreshToken, DateTime TokenExpiryTime);