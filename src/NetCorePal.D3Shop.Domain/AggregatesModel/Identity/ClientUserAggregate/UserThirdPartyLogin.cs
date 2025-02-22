using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

public partial record ThirdPartyLoginId : IInt64StronglyTypedId;

public class UserThirdPartyLogin : Entity<ThirdPartyLoginId>
{
    protected UserThirdPartyLogin()
    {
    }

    public ClientUserId UserId { get; private set; } = null!;
    public string Provider { get; private set; } = string.Empty;
    public string AppId { get; private set; } = string.Empty;
    public string OpenId { get; private set; } = string.Empty;
    public DateTime BindTime { get; private set; }
}