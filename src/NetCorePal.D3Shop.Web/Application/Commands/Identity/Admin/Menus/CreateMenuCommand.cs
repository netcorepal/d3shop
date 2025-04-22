using FluentValidation;
using NetCorePal.D3Shop.Admin.Shared.Requests.MenuRequests;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Queries;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.Extensions.Primitives;
using NetCorePal.Extensions.Repository;
using System.ComponentModel.Design;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin.Menus;

public record CreateMenuCommand(
    string Name,
    string Path,
    MenuType Type,
    MenuId ParentId,
    string AuthCode,
    string Component,
    string Redirect,
    int Order,
    string Icon,
    int Status,
    CreateMenuMetaRequest Meta)
    : ICommand<MenuId>;

public class CreateMenuCommandValidator : AbstractValidator<CreateMenuCommand>
{
    public CreateMenuCommandValidator(MenuQuery menuQuery)
    {
        RuleFor(u => u.Name).NotEmpty().WithMessage("菜单名称不能为空");
        // RuleFor(u => u.Path).NotEmpty().WithMessage("菜单路径不能为空");
        RuleFor(u => u.Name).MustAsync(async (n, ct) => !await menuQuery.ExistsByNameAsync(n, ct))
            .WithMessage(u => $"该菜单名称已存在，Name={u.Name}");
        //RuleFor(u => u.Path).MustAsync(async (p, ct) => !await menuQuery.ExistsByPathAsync(p, ct))
        //    .WithMessage(u => $"该菜单路径已存在，Path={u.Path}");
    }
}

public class CreateMenuCommandHandler(IMenuRepository menuRepository)
    : ICommandHandler<CreateMenuCommand, MenuId>
{
    public async Task<MenuId> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
    {
        var menu = new Menu(
            request.Name,
            request.Path,
            request.Type,
            request.ParentId,
            request.AuthCode,
            request.Component,
            request.Redirect,
            request.Order,
            request.Icon,
            request.Status,
            new MenuMeta
            {
                ActiveIcon = request.Meta.ActiveIcon,
                ActivePath = request.Meta.ActivePath,
                AffixTab = request.Meta.AffixTab,
                AffixTabOrder = request.Meta.AffixTabOrder,
                Badge = request.Meta.Badge,
                BadgeType = request.Meta.BadgeType,
                BadgeVariants = request.Meta.BadgeVariants,
                HideChildrenInMenu = request.Meta.HideChildrenInMenu,
                HideInBreadcrumb = request.Meta.HideInBreadcrumb,
                HideInMenu = request.Meta.HideInMenu,
                HideInTab = request.Meta.HideInTab,
                Icon = request.Meta.Icon,
                IframeSrc = request.Meta.IframeSrc,
                KeepAlive = request.Meta.KeepAlive,
                Link = request.Meta.Link,
                MaxNumOfOpenTab = request.Meta.MaxNumOfOpenTab,
                NoBasicLayout = request.Meta.NoBasicLayout,
                OpenInNewWindow = request.Meta.OpenInNewWindow,
                Order = request.Meta.Order,
                Query = request.Meta.Query,
                Title = request.Meta.Title
            }
        );
        await menuRepository.AddAsync(menu, cancellationToken);
        return menu.Id;
    }
}