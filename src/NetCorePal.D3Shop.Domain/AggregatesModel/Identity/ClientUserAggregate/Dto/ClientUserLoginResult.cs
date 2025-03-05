namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate.Dto;

public class ClientUserLoginResult
{
    public bool IsSuccess { get; init; }
    public string FailedMessage { get; init; } = string.Empty;

    public static ClientUserLoginResult Success()
    {
        return new ClientUserLoginResult { IsSuccess = true };
    }

    public static ClientUserLoginResult Failure(string message)
    {
        return new ClientUserLoginResult { IsSuccess = false, FailedMessage = message };
    }
}