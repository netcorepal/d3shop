namespace NetCorePal.D3Shop.Web.Controllers.Identity.Responses;

public record TokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
