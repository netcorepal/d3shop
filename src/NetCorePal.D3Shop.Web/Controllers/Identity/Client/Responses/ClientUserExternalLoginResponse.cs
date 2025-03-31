namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client.Responses;

public class ClientUserExternalLoginResponse
{
    public bool IsSuccess { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime TokenExpiryTime { get; set; }
    public bool RequiresSignUp { get; set; }
    public string SignupToken { get; set; } = string.Empty;

    public static ClientUserExternalLoginResponse NeedSignUp(string signupToken)
    {
        return new ClientUserExternalLoginResponse
            { RequiresSignUp = true, SignupToken = signupToken };
    }

    public static ClientUserExternalLoginResponse Success(string accessToken, string refreshToken,
        DateTime tokenExpiryTime)
    {
        return new ClientUserExternalLoginResponse
        {
            IsSuccess = true, AccessToken = accessToken, RefreshToken = refreshToken, TokenExpiryTime = tokenExpiryTime
        };
    }
}