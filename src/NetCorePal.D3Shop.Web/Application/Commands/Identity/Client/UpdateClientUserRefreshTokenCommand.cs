using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;

public record UpdateClientUserRefreshTokenCommand(
    ClientUserId UserId,
    string OldRefreshToken,
    string NewRefreshToken) : ICommand;

public class UpdateClientUserRefreshTokenCommandValidator : AbstractValidator<UpdateClientUserRefreshTokenCommand>
{
}

public class UpdateClientUserRefreshTokenCommandHandler(IClientUserRepository clientUserRepository)
    : ICommandHandler<UpdateClientUserRefreshTokenCommand>
{
    public async Task Handle(
        UpdateClientUserRefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var user = await clientUserRepository.GetIncludeRefreshTokensAsync(request.UserId, cancellationToken) ??
                   throw new KnownException($"未找到用户，UserId = {request.UserId}");

        user.RefreshToken(request.OldRefreshToken, request.NewRefreshToken);
    }
}