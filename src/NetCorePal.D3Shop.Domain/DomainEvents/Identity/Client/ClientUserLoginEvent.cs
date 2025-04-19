using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Domain.DomainEvents.Identity.Client;

public record ClientUserLoginEvent(
    ClientUserId UserId,
    string NickName,
    DateTimeOffset LoginTime,
    string LoginMethod,
    string IpAddress,
    string UserAgent) : IDomainEvent;