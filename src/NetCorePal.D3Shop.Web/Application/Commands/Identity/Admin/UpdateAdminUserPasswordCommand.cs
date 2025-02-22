using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;

public record UpdateAdminUserPasswordCommand(AdminUserId AdminUserId, string Password) : ICommand;

public class UpdateAdminUserPasswordCommandHandler(AdminUserRepository adminUserRepository)
    : ICommandHandler<UpdateAdminUserPasswordCommand>
{
    public async Task Handle(UpdateAdminUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await adminUserRepository.GetAsync(request.AdminUserId, cancellationToken) ??
                   throw new KnownException($"未找到用户，AdminUserId = {request.AdminUserId}");

        user.SetPassword(request.Password);
    }
}