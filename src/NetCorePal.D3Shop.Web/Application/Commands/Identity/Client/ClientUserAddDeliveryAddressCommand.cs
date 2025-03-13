using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;

public record ClientUserAddDeliveryAddressCommand(
    ClientUserId UserId,
    string Address,
    string RecipientName,
    string Phone,
    bool SetAsDefault) : ICommand<DeliveryAddressId>;

public class ClientUserAddDeliveryAddressCommandValidator : AbstractValidator<ClientUserAddDeliveryAddressCommand>
{
}

public class ClientUserAddDeliveryAddressCommandHandler(ClientUserRepository clientUserRepository)
    : ICommandHandler<ClientUserAddDeliveryAddressCommand, DeliveryAddressId>
{
    public async Task<DeliveryAddressId> Handle(ClientUserAddDeliveryAddressCommand request,
        CancellationToken cancellationToken)
    {
        var user = await clientUserRepository.GetAsync(request.UserId, cancellationToken) ??
                   throw new KnownException($"用户不存在，UserId={request.UserId}");
        return user.AddDeliveryAddress(request.Address, request.RecipientName, request.Phone, request.SetAsDefault);
    }
}