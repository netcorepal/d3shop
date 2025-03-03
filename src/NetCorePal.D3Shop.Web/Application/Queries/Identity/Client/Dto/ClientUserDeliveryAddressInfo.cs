using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity.Client.Dto;

public record ClientUserDeliveryAddressInfo(
    DeliveryAddressId Id,
    string Address,
    string RecipientName,
    string Phone,
    bool IsDefault);