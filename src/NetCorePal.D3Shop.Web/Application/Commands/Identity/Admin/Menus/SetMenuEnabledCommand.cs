using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.Extensions.Primitives;
using NetCorePal.Extensions.Repository;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin.Menus;

public record SetMenuEnabledCommand(MenuId Id, bool IsEnabled) : ICommand;

public class SetMenuEnabledCommandHandler(IMenuRepository menuRepository)
    : ICommandHandler<SetMenuEnabledCommand>
{
    public async Task Handle(SetMenuEnabledCommand request, CancellationToken cancellationToken)
    {
        var menu = await menuRepository.GetAsync(request.Id, cancellationToken);
        if (menu == null)
        {
            throw new InvalidOperationException($"菜单不存在，Id={request.Id}");
        }

        menu.SetEnabled(request.IsEnabled);
        await menuRepository.UpdateAsync(menu, cancellationToken);
    }
}