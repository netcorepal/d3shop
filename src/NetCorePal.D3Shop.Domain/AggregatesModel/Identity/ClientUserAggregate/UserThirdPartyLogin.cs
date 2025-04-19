using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

public partial record ThirdPartyLoginId : IInt64StronglyTypedId;

public class UserThirdPartyLogin : Entity<ThirdPartyLoginId>
{
    protected UserThirdPartyLogin()
    {
    }

    public UserThirdPartyLogin(
        ClientUserId userId,
        ThirdPartyProvider provider,
        string appId,
        string openId)
    {
        UserId = userId;
        Provider = provider;
        AppId = appId;
        OpenId = openId;
        BindTime = DateTimeOffset.UtcNow;
    }

    public ClientUserId UserId { get; private set; } = null!;
    public ThirdPartyProvider Provider { get; private set; }
    public string AppId { get; private set; } = string.Empty;
    public string OpenId { get; private set; } = string.Empty;
    public DateTimeOffset BindTime { get; private set; }

    /// <summary>
    ///     更新OpenId（重新授权后获取新标识）
    /// </summary>
    /// <param name="newOpenId"></param>
    internal void UpdateOpenId(string newOpenId)
    {
        OpenId = newOpenId;
    }
}