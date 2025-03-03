using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client.Requests;

public record AddDeliveryAddressRequest(
    ClientUserId UserId,
    string Address,
    string RecipientName,
    string Phone,
    bool SetAsDefault);