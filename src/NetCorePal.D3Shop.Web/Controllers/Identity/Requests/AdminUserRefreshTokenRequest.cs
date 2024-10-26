namespace NetCorePal.D3Shop.Web.Controllers.Identity.Requests;

public record AdminUserRefreshTokenRequest(string Token, string RefreshToken);