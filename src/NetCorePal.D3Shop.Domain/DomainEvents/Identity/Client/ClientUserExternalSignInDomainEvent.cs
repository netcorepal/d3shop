using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Domain.DomainEvents.Identity.Client;

public record ClientUserExternalSignInDomainEvent(
    ClientUserId UserId,
    string NickName,
    DateTime LoginTime,
    string LoginMethod,
    string IpAddress,
    string UserAgent) : IDomainEvent;