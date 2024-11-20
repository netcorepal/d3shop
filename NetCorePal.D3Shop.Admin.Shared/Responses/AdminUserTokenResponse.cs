namespace NetCorePal.D3Shop.Admin.Shared.Responses;

public record AdminUserTokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
