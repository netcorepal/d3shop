using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client.Requests;

public record UpdateDeliveryAddressRequest(
    DeliveryAddressId DeliveryAddressId,
    string Address,
    string RecipientName,
    string Phone,
    bool SetAsDefault);