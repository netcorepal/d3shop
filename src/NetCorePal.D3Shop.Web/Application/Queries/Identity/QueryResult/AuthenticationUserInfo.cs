using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity.QueryResult;

public record AuthenticationUserInfo(AdminUserId Id,string Name,string Password,string Phone);