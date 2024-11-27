using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity.QueryResult;

public record AdminUserCredentials(AdminUserId Id, string Password);