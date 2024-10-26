namespace NetCorePal.D3Shop.Web.Controllers.Identity.Responses;

public record AminUserTokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
