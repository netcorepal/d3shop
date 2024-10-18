using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Requests;

public record CreateAdminUserRequest(string Name, string Phone, string PassWord, IEnumerable<RoleId> RoleIds);