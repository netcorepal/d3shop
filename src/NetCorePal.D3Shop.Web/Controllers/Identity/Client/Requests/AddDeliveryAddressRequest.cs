namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client.Requests;

public record AddDeliveryAddressRequest(
    string Address,
    string RecipientName,
    string Phone,
    bool SetAsDefault);