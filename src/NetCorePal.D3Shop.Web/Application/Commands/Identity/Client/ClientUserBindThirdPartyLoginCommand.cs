using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;

public record ClientUserBindThirdPartyLoginCommand(
    ClientUserId UserId,
    ThirdPartyProvider ThirdPartyProvider,
    string AppId,
    string OpenId) : ICommand;

public class ClientUserBindThirdPartyLoginCommandValidator : AbstractValidator<ClientUserBindThirdPartyLoginCommand>
{
    public ClientUserBindThirdPartyLoginCommandValidator()
    {
        RuleFor(x => x.ThirdPartyProvider).NotEmpty().WithMessage("第三方登录类型不能为空");
        RuleFor(x => x.AppId).NotEmpty().WithMessage("AppId不能为空");
        RuleFor(x => x.OpenId).NotEmpty().WithMessage("OpenId不能为空");
    }
}

public class ClientUserBindThirdPartyLoginCommandHandler(ClientUserRepository clientUserRepository)
    : ICommandHandler<ClientUserBindThirdPartyLoginCommand>
{
    public async Task Handle(ClientUserBindThirdPartyLoginCommand request, CancellationToken cancellationToken)
    {
        var user = await clientUserRepository.GetAsync(request.UserId, cancellationToken) ??
                   throw new KnownException("用户不存在");
        user.BindThirdPartyLogin(request.ThirdPartyProvider, request.AppId, request.OpenId);
    }
}