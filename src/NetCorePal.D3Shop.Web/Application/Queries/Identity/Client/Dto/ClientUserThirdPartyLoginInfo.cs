using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity.Client.Dto;

public record ClientUserThirdPartyLoginInfo(
    ThirdPartyLoginId ThirdPartyLoginId,
    ThirdPartyProvider ThirdPartyProvider
);