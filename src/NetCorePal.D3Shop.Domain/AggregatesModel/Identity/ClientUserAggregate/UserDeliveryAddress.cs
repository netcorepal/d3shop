using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

public partial record DeliveryAddressId : IInt64StronglyTypedId;

public class UserDeliveryAddress : Entity<DeliveryAddressId>
{
    protected UserDeliveryAddress()
    {
    }

    public UserDeliveryAddress(
        ClientUserId userId,
        string province,
        string provinceCode,
        string city,
        string cityCode,
        string district,
        string districtCode,
        string address,
        string recipientName,
        string phone,
        bool isDefault)
    {
        UserId = userId;
        Province = province;
        ProvinceCode = provinceCode;
        City = city;
        CityCode = cityCode;
        District = district;
        DistrictCode = districtCode;
        Address = address;
        RecipientName = recipientName;
        Phone = phone;
        IsDefault = isDefault;
    }

    public ClientUserId UserId { get; private set; } = null!;
    public string Province { get; private set; } = string.Empty;
    public string ProvinceCode { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string CityCode { get; private set; } = string.Empty;
    public string District { get; private set; } = string.Empty;
    public string DistrictCode { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public string RecipientName { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public bool IsDefault { get; private set; }

    internal void UpdateDetails(
        string province,
        string provinceCode,
        string city,
        string cityCode,
        string district,
        string districtCode,
        string address,
        string recipientName,
        string phone)
    {
        Province = province;
        ProvinceCode = provinceCode;
        City = city;
        CityCode = cityCode;
        District = district;
        DistrictCode = districtCode;
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