namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client.Responses;

public class ClientUserLoginResponse
{
    public bool IsSuccess { get; init; }
    public string? Token { get; init; }
    public string FailedMessage { get; init; } = string.Empty;

    public static ClientUserLoginResponse Success(string token)
    {
        return new ClientUserLoginResponse { IsSuccess = true, Token = token };
    }

    public static ClientUserLoginResponse Failure(string message)
    {
        return new ClientUserLoginResponse { IsSuccess = false, FailedMessage = message };
    }
}