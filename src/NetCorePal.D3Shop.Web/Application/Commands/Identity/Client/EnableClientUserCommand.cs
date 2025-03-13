using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;

public record EnableClientUserCommand(ClientUserId UserId) : ICommand;

public class EnableClientUserCommandHandle(ClientUserRepository clientUserRepository)
    : ICommandHandler<EnableClientUserCommand>
{
    public async Task Handle(EnableClientUserCommand request, CancellationToken cancellationToken)
    {
        var user = await clientUserRepository.GetAsync(request.UserId, cancellationToken) ??
                   throw new KnownException($"未找到用户，UserId = {request.UserId}");
        user.Enable();
    }
}