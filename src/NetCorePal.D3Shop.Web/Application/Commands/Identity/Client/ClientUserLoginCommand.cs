using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;

public record ClientUserLoginCommand(
    ClientUserId UserId,
    string PasswordHash,
    DateTime LoginTime,
    string LoginMethod,
    string IpAddress,
    string UserAgent) : ICommand;

public class ClientUserLoginCommandHandler(IClientUserRepository clientUserRepository)
    : ICommandHandler<ClientUserLoginCommand>
{
    public async Task Handle(ClientUserLoginCommand request, CancellationToken cancellationToken)
    {
        var user = await clientUserRepository.GetAsync(request.UserId, cancellationToken) ??
                   throw new KnownException("用户不存在");
        user.Login(request.PasswordHash, request.LoginTime, request.LoginMethod, request.IpAddress, request.UserAgent);
    }
}