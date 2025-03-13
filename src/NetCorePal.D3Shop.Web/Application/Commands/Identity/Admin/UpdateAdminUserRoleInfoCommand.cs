using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;

public record UpdateAdminUserRoleInfoCommand(AdminUserId AdminUserId, RoleId RoleId, string RoleName) : ICommand;

public class UpdateAdminUserRoleInfoCommandHandler(AdminUserRepository adminUserRepository)
    : ICommandHandler<UpdateAdminUserRoleInfoCommand>
{
    public async Task Handle(UpdateAdminUserRoleInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await adminUserRepository.GetAsync(request.AdminUserId, cancellationToken) ??
                   throw new KnownException($"未找到用户，AdminUserId = {request.AdminUserId}");

        user.UpdateRoleInfo(request.RoleId, request.RoleName);
    }
}