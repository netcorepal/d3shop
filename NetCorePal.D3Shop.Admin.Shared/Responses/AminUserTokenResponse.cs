namespace NetCorePal.D3Shop.Admin.Shared.Responses;

public record AminUserTokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
