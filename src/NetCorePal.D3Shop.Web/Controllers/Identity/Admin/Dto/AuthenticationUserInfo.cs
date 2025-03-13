using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Admin.Dto;

public record AuthenticationUserInfo(AdminUserId Id,string Name,string Password,string Phone);