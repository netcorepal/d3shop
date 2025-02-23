using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;

public record ClientUserUpdateDeliveryAddressCommand(
    ClientUserId UserId,
    DeliveryAddressId DeliveryAddressId,
    string Address,
    string RecipientName,
    string Phone,
    bool SetAsDefault) : ICommand;

public class ClientUserUpdateDeliveryAddressCommandValidator : AbstractValidator<ClientUserUpdateDeliveryAddressCommand>
{
}

public class ClientUserUpdateDeliveryAddressCommandHandler(ClientUserRepository clientUserRepository)
    : ICommandHandler<ClientUserUpdateDeliveryAddressCommand>
{
    public async Task Handle(ClientUserUpdateDeliveryAddressCommand request, CancellationToken cancellationToken)
    {
        var user = await clientUserRepository.GetAsync(request.UserId, cancellationToken) ??
                   throw new KnownException($"用户不存在，UserId={request.UserId}");
        user.UpdateDeliveryAddress(request.DeliveryAddressId, request.Address, request.RecipientName,
            request.Phone, request.SetAsDefault);
    }
}