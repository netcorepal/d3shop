using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Domain.DomainEvents.Identity;

public record RoleInfoChangedDomainEvent(Role Role) : IDomainEvent;