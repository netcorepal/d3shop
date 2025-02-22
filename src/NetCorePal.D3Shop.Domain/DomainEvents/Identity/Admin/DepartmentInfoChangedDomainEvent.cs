using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Domain.DomainEvents.Identity.Admin;

public record DepartmentInfoChangedDomainEvent(Department Department) : IDomainEvent;