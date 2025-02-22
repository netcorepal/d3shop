using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

public partial record DeliveryAddressId : IInt64StronglyTypedId;

public class UserDeliveryAddress : Entity<DeliveryAddressId>
{
    protected UserDeliveryAddress()
    {
    }

    internal UserDeliveryAddress(
        ClientUserId userId,
        string address,
        string recipientName,
        string phone,
        bool isDefault)
    {
        UserId = userId;
        Address = address;
        RecipientName = recipientName;
        Phone = phone;
        IsDefault = isDefault;
    }

    public ClientUserId UserId { get; private set; } = null!;
    public string Address { get; private set; } = string.Empty;
    public string RecipientName { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public bool IsDefault { get; private set; }
    
    internal void UpdateDetails(string address, string recipientName, string phone)
    {
        Address = address;
        RecipientName = recipientName;
        Phone = phone;
    }

    internal void SetAsDefault()
    {
        IsDefault = true;
    }

    internal void UnsetDefault()
    {
        IsDefault = false;
    }
}