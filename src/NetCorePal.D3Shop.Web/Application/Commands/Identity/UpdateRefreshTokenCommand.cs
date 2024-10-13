using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity;

public record UpdateRefreshTokenCommand(AdminUserId AdminUserId, string RefreshToken) : ICommand;

public class UpdateRefreshTokenCommandHandler(AdminUserRepository adminUserRepository) : ICommandHandler<UpdateRefreshTokenCommand>
{
    public async Task Handle(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await adminUserRepository.GetAsync(request.AdminUserId, cancellationToken) ??
                   throw new KnownException($"未找到用户，AdminUserId = {request.AdminUserId}");

        user.SetRefreshToken(request.RefreshToken);
    }
}
