using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;

public record CreateClientUserCommand(
    string NickName,
    string Avatar,
    string Phone,
    string PasswordHash,
    string PasswordSalt,
    string Email) : ICommand<ClientUserId>;

public class CreateClientUserCommandValidator : AbstractValidator<CreateClientUserCommand>
{
    public CreateClientUserCommandValidator(ClientUserRepository clientUserRepository)
    {
        RuleFor(x => x.NickName).NotEmpty().WithMessage("昵称不能为空");
        RuleFor(x => x.Phone).NotEmpty().WithMessage("手机号不能为空");
        RuleFor(x => x.PasswordHash).NotEmpty().WithMessage("密码不能为空");
    }
}

public class CreateClientUserCommandHandle(IClientUserRepository clientUserRepository)
    : ICommandHandler<CreateClientUserCommand, ClientUserId>
{
    public async Task<ClientUserId> Handle(CreateClientUserCommand request, CancellationToken cancellationToken)
    {
        var user = new ClientUser(
            request.NickName,
            request.Avatar,
            request.Phone,
            request.PasswordHash,
            request.PasswordSalt,
            request.Email);
        await clientUserRepository.AddAsync(user, cancellationToken);
        return user.Id;
    }
}