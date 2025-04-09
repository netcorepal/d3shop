using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Queries;
using NetCorePal.Extensions.Primitives;
using NetCorePal.Extensions.Repository;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.VueAdmin;

public record UpdateMenuCommand(
    MenuId Id,
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
    : ICommand;

public class UpdateMenuCommandValidator : AbstractValidator<UpdateMenuCommand>
{
    public UpdateMenuCommandValidator(MenuQuery menuQuery)
    {
        RuleFor(u => u.Id).NotEmpty().WithMessage("菜单ID不能为空");
        RuleFor(u => u.Name).NotEmpty().WithMessage("菜单名称不能为空");
        RuleFor(u => u.Path).NotEmpty().WithMessage("菜单路径不能为空");
        RuleFor(u => u.Name).MustAsync(async (command, name, ct) =>
            !await menuQuery.ExistsByNameAsync(name, command.Id, ct))
            .WithMessage(u => $"该菜单名称已存在，Name={u.Name}");
        RuleFor(u => u.Path).MustAsync(async (command, path, ct) =>
            !await menuQuery.ExistsByPathAsync(path, command.Id, ct))
            .WithMessage(u => $"该菜单路径已存在，Path={u.Path}");
    }
}

public class UpdateMenuCommandHandler(IMenuRepository menuRepository)
    : ICommandHandler<UpdateMenuCommand>
{
    public async Task Handle(UpdateMenuCommand request, CancellationToken cancellationToken)
    {
        var menu = await menuRepository.GetAsync(request.Id, cancellationToken);
        if (menu == null)
        {
            throw new InvalidOperationException($"菜单不存在，Id={request.Id}");
        }

        menu.Update(
            request.Name,
            request.Path,
            request.Type,
            request.ParentId,
            request.AuthCode,
            request.Component,
            request.Redirect,
            request.Order,
            request.Icon,//request.Icon,待优化
            request.Status,
            request.Meta
        );
    }
}