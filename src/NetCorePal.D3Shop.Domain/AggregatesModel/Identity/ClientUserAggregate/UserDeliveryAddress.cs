using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

public partial record DeliveryAddressId : IInt64StronglyTypedId;

public class UserDeliveryAddress : Entity<DeliveryAddressId>
{
    protected UserDeliveryAddress()
    {
    }

    public ClientUserId UserId { get; private set; } = null!;
    public string Address { get; private set; } = string.Empty;
    public string RecipientName { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public bool IsDefault { get; private set; }
}