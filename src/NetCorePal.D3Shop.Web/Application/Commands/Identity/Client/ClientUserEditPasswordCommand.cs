using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;

public record ClientUserEditPasswordCommand(
    ClientUserId UserId,
    string OldPasswordHash,
    string NewPasswordHash) : ICommand;

public class ClientUserEditPasswordCommandValidator : AbstractValidator<ClientUserEditPasswordCommand>
{
    public ClientUserEditPasswordCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("用户Id不能为空");
        RuleFor(x => x.OldPasswordHash).NotEmpty().WithMessage("旧密码不能为空");
        RuleFor(x => x.NewPasswordHash).NotEmpty().WithMessage("新密码不能为空");
    }
}

public class ClientUserEditPasswordCommandHandler(ClientUserRepository clientUserRepository)
    : ICommandHandler<ClientUserEditPasswordCommand>
{
    public async Task Handle(ClientUserEditPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await clientUserRepository.GetAsync(request.UserId, cancellationToken) ??
                   throw new KnownException($"用户不存在，UserId={request.UserId}");
        user.EditPassword(request.OldPasswordHash, request.NewPasswordHash);
    }
}