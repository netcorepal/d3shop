using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Domain.DomainEvents.Identity;

public record DepartmentInfoChangedDomainEvent(Department Department) : IDomainEvent;