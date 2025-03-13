using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate.Dto;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;

public record ClientUserLoginCommand(
    ClientUserId UserId,
    string PasswordHash,
    DateTime LoginTime,
    string LoginMethod,
    string IpAddress,
    string UserAgent,
    string RefreshToken) : ICommand<ClientUserLoginResult>;

public class ClientUserLoginCommandHandler(IClientUserRepository clientUserRepository)
    : ICommandHandler<ClientUserLoginCommand, ClientUserLoginResult>
{
    public async Task<ClientUserLoginResult> Handle(ClientUserLoginCommand request, CancellationToken cancellationToken)
    {
        var user = await clientUserRepository.GetAsync(request.UserId, cancellationToken) ??
                   throw new KnownException("用户不存在");

        return user.Login(
            request.PasswordHash,
            request.LoginTime,
            request.LoginMethod,
            request.IpAddress,
            request.UserAgent,
            request.RefreshToken);
    }
}