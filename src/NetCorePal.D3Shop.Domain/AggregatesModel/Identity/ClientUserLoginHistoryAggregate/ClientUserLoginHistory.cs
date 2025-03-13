using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserLoginHistoryAggregate;

public partial record ClientUserLoginHistoryId : IInt64StronglyTypedId;

public class ClientUserLoginHistory : Entity<ClientUserLoginHistoryId>, IAggregateRoot
{
    protected ClientUserLoginHistory()
    {
    }

    public ClientUserLoginHistory(
        ClientUserId userId,
        string nickName,
        DateTime loginTime,
        string loginMethod,
        string ipAddress,
        string userAgent)
    {
        UserId = userId;
        NickName = nickName;
        LoginTime = loginTime;
        LoginMethod = loginMethod;
        IpAddress = ipAddress;
        UserAgent = userAgent;
    }

    public ClientUserId UserId { get; private set; } = null!;
    public string NickName { get; private set; } = string.Empty;
    public DateTime LoginTime { get; private set; }
    public string LoginMethod { get; private set; } = string.Empty;
    public string IpAddress { get; private set; } = string.Empty;
    public string UserAgent { get; private set; } = string.Empty;
}