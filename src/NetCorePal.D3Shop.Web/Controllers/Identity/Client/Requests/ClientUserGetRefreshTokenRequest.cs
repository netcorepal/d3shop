namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client.Requests;

public record ClientUserGetRefreshTokenRequest(
    string Token,
    string RefreshToken);