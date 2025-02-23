using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;

public record ClientUserUnbindThirdPartyLoginCommand(
    ClientUserId UserId,
    ThirdPartyLoginId ThirdPartyLoginId) : ICommand;

public class ClientUserUnbindThirdPartyLoginCommandValidator : AbstractValidator<ClientUserUnbindThirdPartyLoginCommand>
{
    public ClientUserUnbindThirdPartyLoginCommandValidator()
    {
        RuleFor(x => x.ThirdPartyLoginId).NotEmpty().WithMessage("第三方登录类型不能为空");
    }
}

public class ClientUserUnbindThirdPartyLoginCommandHandler(ClientUserRepository clientUserRepository)
    : ICommandHandler<ClientUserUnbindThirdPartyLoginCommand>
{
    public async Task Handle(ClientUserUnbindThirdPartyLoginCommand request, CancellationToken cancellationToken)
    {
        var user = await clientUserRepository.GetAsync(request.UserId, cancellationToken) ??
                   throw new KnownException("用户不存在");
        user.UnbindThirdPartyLogin(request.ThirdPartyLoginId);
    }
}