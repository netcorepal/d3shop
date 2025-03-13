using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Admin.Dto;

public record AdminUserCredentials(AdminUserId Id, string Password);