namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client.Responses;

public class ClientUserLoginResponse
{
    public bool IsSuccess { get; set; }
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string FailedMessage { get; set; } = string.Empty;

    public static ClientUserLoginResponse Success(string token, string refreshToken)
    {
        return new ClientUserLoginResponse { IsSuccess = true, Token = token, RefreshToken = refreshToken };
    }

    public static ClientUserLoginResponse Failure(string message)
    {
        return new ClientUserLoginResponse { IsSuccess = false, FailedMessage = message };
    }
}