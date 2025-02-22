using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;

public record DeleteAdminUserCommand(AdminUserId AdminUserId) : ICommand;

public class DeleteAdminUserCommandHandler(IAdminUserRepository adminUserRepository)
    : ICommandHandler<DeleteAdminUserCommand>
{
    public async Task Handle(DeleteAdminUserCommand request, CancellationToken cancellationToken)
    {
        var adminUser = await adminUserRepository.GetAsync(request.AdminUserId, cancellationToken) ??
                        throw new KnownException($"用户不存在，AdminUserId={request.AdminUserId}");
        if (adminUser.Name == AppDefaultCredentials.Name)
            throw new KnownException("无法删除默认用户");

        adminUser.Delete();
    }
}