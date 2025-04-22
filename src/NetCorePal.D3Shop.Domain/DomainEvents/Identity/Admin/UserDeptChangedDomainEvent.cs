using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Domain.DomainEvents.Identity.Admin;

/// <summary>
/// 用户部门改变事件
/// </summary>
public record UserDeptChangedDomainEvent(UserDept UserDept) : IDomainEvent; 