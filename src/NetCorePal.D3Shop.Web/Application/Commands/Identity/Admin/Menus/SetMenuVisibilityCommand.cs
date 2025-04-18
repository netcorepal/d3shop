using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.Extensions.Primitives;
using NetCorePal.Extensions.Repository;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin.Menus;

public record SetMenuVisibilityCommand(MenuId Id, bool IsVisible) : ICommand;

public class SetMenuVisibilityCommandHandler(IMenuRepository menuRepository)
    : ICommandHandler<SetMenuVisibilityCommand>
{
    public async Task Handle(SetMenuVisibilityCommand request, CancellationToken cancellationToken)
    {
        var menu = await menuRepository.GetAsync(request.Id, cancellationToken);
        if (menu == null)
        {
            throw new InvalidOperationException($"菜单不存在，Id={request.Id}");
        }

        menu.SetVisibility(request.IsVisible);
        await menuRepository.UpdateAsync(menu, cancellationToken);
    }
}