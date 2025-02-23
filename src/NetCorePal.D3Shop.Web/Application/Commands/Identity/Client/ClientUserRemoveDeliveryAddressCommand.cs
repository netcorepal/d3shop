using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;

public record ClientUserRemoveDeliveryAddressCommand(
    ClientUserId UserId,
    DeliveryAddressId DeliveryAddressId) : ICommand;

public class ClientUserRemoveDeliveryAddressCommandHandler(ClientUserRepository clientUserRepository)
    : ICommandHandler<ClientUserRemoveDeliveryAddressCommand>
{
    public async Task Handle(ClientUserRemoveDeliveryAddressCommand request, CancellationToken cancellationToken)
    {
        var user = await clientUserRepository.GetAsync(request.UserId, cancellationToken) ??
                   throw new KnownException($"用户不存在，UserId={request.UserId}");
        user.RemoveDeliveryAddress(request.DeliveryAddressId);
    }
}