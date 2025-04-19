namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client.Responses;

public record ClientUserLoginResponse(
    string Token = "",
    string RefreshToken = "",
    DateTimeOffset TokenExpiryTime = default
);