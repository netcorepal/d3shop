namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client.Requests;

public record AddDeliveryAddressRequest(
    string Province,
    string ProvinceCode,
    string City,
    string CityCode,
    string District,
    string DistrictCode,
    string Address,
    string RecipientName,
    string Phone,
    bool SetAsDefault);