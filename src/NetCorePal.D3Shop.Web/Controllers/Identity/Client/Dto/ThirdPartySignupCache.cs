using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client.Dto;

public record ThirdPartySignupCache(
    ThirdPartyProvider ThirdPartyProvider,
    string AppId,
    string OpenId,
    string IpAddress,
    string UserAgent,
    DateTime AuthTime);