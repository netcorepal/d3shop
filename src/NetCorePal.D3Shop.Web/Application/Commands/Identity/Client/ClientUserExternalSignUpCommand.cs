using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Client;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;

public record ClientUserExternalSignUpCommand(
    DateTimeOffset SignUpTime,
    string Phone,
    string PasswordHash,
    string PasswordSalt,
    ThirdPartyProvider ThirdPartyProvider,
    string AppId,
    string OpenId,
    string RefreshToken,
    string IpAddress,
    string UserAgent
) : ICommand<ClientUserId>;

public class
    ClientUserExternalSignUpCommandCommandValidator : AbstractValidator<ClientUserExternalSignUpCommand>
{
    public ClientUserExternalSignUpCommandCommandValidator(ClientUserQuery clientUserQuery)
    {
        RuleFor(x => x.Phone).NotEmpty().WithMessage("手机号不能为空");
        RuleFor(x => x.Phone)
            .MustAsync(async (phone, cancellationToken) =>
                !await clientUserQuery.DoesPhoneExistAsync(phone, cancellationToken)).WithMessage("手机号已经被注册");
    }
}

public class ClientUserExternalSignUpCommandHandler(ClientUserRepository clientUserRepository)
    : ICommandHandler<ClientUserExternalSignUpCommand, ClientUserId>
{
    public async Task<ClientUserId> Handle(
        ClientUserExternalSignUpCommand request,
        CancellationToken cancellationToken)
    {
        var user = ClientUser.ExternalSignUp(
            request.SignUpTime,
            request.Phone,
            request.PasswordHash,
            request.PasswordSalt,
            request.ThirdPartyProvider,
            request.AppId,
            request.OpenId,
            request.RefreshToken,
            request.IpAddress,
            request.UserAgent);

        await clientUserRepository.AddAsync(user, cancellationToken);
        return user.Id;
    }
}