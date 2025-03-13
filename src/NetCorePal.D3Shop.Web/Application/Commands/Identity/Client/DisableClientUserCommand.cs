using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;

public record DisableClientUserCommand(
    ClientUserId UserId,
    string Reason) : ICommand;

public class DisableClientUserCommandHandle(ClientUserRepository clientUserRepository)
    : ICommandHandler<DisableClientUserCommand>
{
    public async Task Handle(DisableClientUserCommand request, CancellationToken cancellationToken)
    {
        var user = await clientUserRepository.GetAsync(request.UserId, cancellationToken) ??
                   throw new KnownException($"未找到用户，UserId = {request.UserId}");
        user.Disable(request.Reason);
    }
}