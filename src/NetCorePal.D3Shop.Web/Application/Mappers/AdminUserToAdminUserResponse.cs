using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.Extensions.Mappers;

namespace NetCorePal.D3Shop.Web.Application.Mappers;

public class AdminUserToAdminUserResponse : IMapper<AdminUser, AdminUserResponse>
{
    public AdminUserResponse To(AdminUser from)
    {
        return new AdminUserResponse(from.Id, from.Name, from.Phone, from.Roles, from.Permissions);
    }
}