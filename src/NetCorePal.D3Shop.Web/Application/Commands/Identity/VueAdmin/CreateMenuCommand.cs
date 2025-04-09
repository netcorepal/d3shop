using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Queries;
using NetCorePal.Extensions.Primitives;
using NetCorePal.Extensions.Repository;
using System.ComponentModel.Design;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.VueAdmin;

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
    MenuMeta Meta)
    : ICommand<MenuId>;

public class CreateMenuCommandValidator : AbstractValidator<CreateMenuCommand>
{
    public CreateMenuCommandValidator(MenuQuery menuQuery)
    {
        RuleFor(u => u.Name).NotEmpty().WithMessage("菜单名称不能为空");
        RuleFor(u => u.Path).NotEmpty().WithMessage("菜单路径不能为空");
        RuleFor(u => u.Name).MustAsync(async (n, ct) => !await menuQuery.ExistsByNameAsync(n, ct))
            .WithMessage(u => $"该菜单名称已存在，Name={u.Name}");
        RuleFor(u => u.Path).MustAsync(async (p, ct) => !await menuQuery.ExistsByPathAsync(p, ct))
            .WithMessage(u => $"该菜单路径已存在，Path={u.Path}");
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
            request.Meta
        );
        await menuRepository.AddAsync(menu, cancellationToken);
        return menu.Id;
    }
}