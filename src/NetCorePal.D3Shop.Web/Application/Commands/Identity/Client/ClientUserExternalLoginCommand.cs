using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;

public record ClientUserExternalLoginCommand(
    ClientUserId UserId,
    DateTime LoginTime,
    string LoginMethod,
    string IpAddress,
    string UserAgent,
    string RefreshToken) : ICommand;

public class ClientUserExternalLoginCommandValidator : AbstractValidator<ClientUserExternalLoginCommand>
{
}

public class ClientUserExternalLoginCommandHandler(ClientUserRepository clientUserRepository)
    : ICommandHandler<ClientUserExternalLoginCommand>
{
    public async Task Handle(
        ClientUserExternalLoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await clientUserRepository.GetAsync(request.UserId, cancellationToken) ??
                   throw new KnownException("用户不存在");

        user.ExternalLogin(
            request.LoginTime,
            request.LoginMethod,
            request.IpAddress,
            request.UserAgent,
            request.RefreshToken);
    }
}