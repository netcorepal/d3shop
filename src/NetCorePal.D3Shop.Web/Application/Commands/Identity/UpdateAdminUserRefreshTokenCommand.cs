using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity;

public record UpdateAdminUserRefreshTokenCommand(AdminUserId AdminUserId, string RefreshToken) : ICommand;

public class UpdateAdminUserRefreshTokenCommandHandler(AdminUserRepository adminUserRepository) : ICommandHandler<UpdateAdminUserRefreshTokenCommand>
{
    public async Task Handle(UpdateAdminUserRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await adminUserRepository.GetAsync(request.AdminUserId, cancellationToken) ??
                   throw new KnownException($"未找到用户，AdminUserId = {request.AdminUserId}");

        user.UpdateRefreshToken(request.RefreshToken);
    }
}
