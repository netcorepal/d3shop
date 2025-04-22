using Consul;
using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;

public record UpdateRoleInfoCommand(RoleId RoleId, string Name, string Description, int Status,
        IEnumerable<(MenuId menuId, string code)> Permissions) : ICommand;

public class UpdateRoleInfoCommandValidator : AbstractValidator<UpdateRoleInfoCommand>
{
    public UpdateRoleInfoCommandValidator(RoleQuery roleQuery)
    {
        RuleFor(x => x.RoleId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => new { x.Name, x.RoleId }).MustAsync(async (r, ct) =>
            !await roleQuery.RoleExistsByNameAsync(r.Name, r.RoleId, ct)
        );
    }
}

public class UpdateRoleInfoCommandHandler(IRoleRepository roleRepository) : ICommandHandler<UpdateRoleInfoCommand>
{
    public async Task Handle(UpdateRoleInfoCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetAsync(request.RoleId, cancellationToken) ??
                   throw new KnownException($"未找到角色，RoleId = {request.RoleId}");
        role.UpdateRoleInfo(request.Name, request.Description, request.Status);

        // 更新角色权限
        var permissions = request.Permissions.Select(perm => new RolePermission(perm.code, perm.menuId));
        role.UpdateRolePermissions(permissions);
    }
}