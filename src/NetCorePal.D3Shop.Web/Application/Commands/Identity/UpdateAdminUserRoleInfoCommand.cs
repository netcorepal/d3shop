using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity;

public record UpdateAdminUserRoleInfoCommand(AdminUserRole AdminUserRole) : ICommand;

public class UpdateAdminUserRoleInfoCommandHandler(AdminUserRepository adminUserRepository)
    : ICommandHandler<UpdateAdminUserRoleInfoCommand>
{
    public async Task Handle(UpdateAdminUserRoleInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await adminUserRepository.GetAsync(request.AdminUserRole.AdminUserId, cancellationToken) ??
                   throw new KnownException($"未找到用户，AdminUserId = {request.AdminUserRole.AdminUserId}");

        user.UpdateRoleInfo(request.AdminUserRole);
    }
}