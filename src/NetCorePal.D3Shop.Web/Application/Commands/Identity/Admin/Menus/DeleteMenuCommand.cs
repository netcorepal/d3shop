using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.Extensions.Primitives;
using NetCorePal.Extensions.Repository;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin.Menus;

public record DeleteMenuCommand(MenuId Id) : ICommand;

public class DeleteMenuCommandHandler(IMenuRepository menuRepository)
    : ICommandHandler<DeleteMenuCommand>
{
    public async Task Handle(DeleteMenuCommand request, CancellationToken cancellationToken)
    {
        var menu = await menuRepository.GetAsync(request.Id, cancellationToken);
        if (menu == null)
        {
            throw new InvalidOperationException($"菜单不存在，Id={request.Id}");
        }

        //if (menu.Children.Count > 0)
        //{
        //    throw new InvalidOperationException($"菜单存在子菜单，无法删除，Id={request.Id}");
        //}

        await menuRepository.RemoveAsync(menu);
    }
}