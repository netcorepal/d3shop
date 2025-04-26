using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;

public record CreateRoleCommand(string Name, string Description, int Status, IEnumerable<(MenuId menuId, string code)> Permissions)
    : ICommand<RoleId>;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator(RoleQuery roleQuery)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("角色名称不能为空");
        RuleFor(x => x.Name).MustAsync(async (n, ct) => !await roleQuery.RoleExistsByNameAsync(n, ct))
            .WithMessage(r => $"角色名称重复，Name={r.Name}");
    }
}

public class CreateRoleCommandHandler(IRoleRepository roleRepository) : ICommandHandler<CreateRoleCommand, RoleId>
{
    public async Task<RoleId> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var permissions = request.Permissions.Select(perm => new RolePermission(perm.code, perm.menuId));

        var role = new Role(request.Name, request.Description, permissions, request.Status);

        await roleRepository.AddAsync(role, cancellationToken);

        return role.Id;
    }
}