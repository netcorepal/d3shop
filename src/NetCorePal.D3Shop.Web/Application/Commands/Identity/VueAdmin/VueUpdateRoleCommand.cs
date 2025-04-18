using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.VueAdmin
{
    public record VueUpdateRoleCommand(
        RoleId RoleId,
        string Name,
        string Description,
        int Status,
        IEnumerable<(MenuId menuId, string code)> Permissions)
        : ICommand;

    public class VueUpdateRoleCommandValidator : AbstractValidator<VueUpdateRoleCommand>
    {
        public VueUpdateRoleCommandValidator(RoleQuery roleQuery)
        {
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("角色ID不能为空");
            RuleFor(x => x.Name).NotEmpty().WithMessage("角色名称不能为空");
            RuleFor(x => new { x.Name, x.RoleId }).MustAsync(async (r, ct) =>
                !await roleQuery.RoleExistsByNameAsync(r.Name, r.RoleId, ct))
                .WithMessage(r => $"角色名称重复，Name={r.Name}");
        }
    }

    public class VueUpdateRoleCommandHandler(IRoleRepository roleRepository)
        : ICommandHandler<VueUpdateRoleCommand>
    {
        public async Task Handle(VueUpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await roleRepository.GetAsync(request.RoleId, cancellationToken) ??
                       throw new KnownException($"未找到角色，RoleId = {request.RoleId}");

            // 更新角色基本信息
            role.UpdateRoleInfo(request.Name, request.Description);
            role.Status = request.Status;

            // 更新角色权限
            var permissions = request.Permissions.Select(perm => new RolePermission(perm.code, perm.menuId));
            role.UpdateRolePermissions(permissions);
        }
    }
}