using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity
{
    public record LoginSuccessfullyCommand(AdminUserId AdminUserId, string RefreshToken, DateTime RefreshTokenExpiryDate) : ICommand;

    public class LoginSuccessfullyCommandCommandHandler(
        IAdminUserRepository adminUserRepository) : ICommandHandler<LoginSuccessfullyCommand>
    {
        public async Task Handle(LoginSuccessfullyCommand request, CancellationToken cancellationToken)
        {
            var user = await adminUserRepository.GetAsync(request.AdminUserId, cancellationToken) ??
                       throw new KnownException($"未找到用户，AdminUserId = {request.AdminUserId}");

            user.SetRefreshToken(request.RefreshToken);
            user.SetRefreshTokenExpiryDate(request.RefreshTokenExpiryDate);
        }
    }
}
