namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client.Responses;

public record ClientUserGetRefreshTokenResponse(
    string Token,
    string RefreshToken,
    DateTime TokenExpiryTime);