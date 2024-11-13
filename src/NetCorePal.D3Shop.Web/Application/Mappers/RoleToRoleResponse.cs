using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.Extensions.Mappers;

namespace NetCorePal.D3Shop.Web.Application.Mappers;

public class RoleToRoleResponse : IMapper<Role, RoleResponse>
{
    public RoleResponse To(Role from)
    {
        return new RoleResponse(from.Id, from.Name, from.Description, from.Permissions);
    }
}