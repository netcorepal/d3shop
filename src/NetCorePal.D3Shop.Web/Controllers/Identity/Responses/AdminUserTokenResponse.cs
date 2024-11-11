namespace NetCorePal.D3Shop.Web.Controllers.Identity.Responses;

public record AdminUserTokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
