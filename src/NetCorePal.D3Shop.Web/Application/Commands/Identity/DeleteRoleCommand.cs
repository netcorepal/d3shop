using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity;
using NetCorePal.D3Shop.Web.Application.Queries.Identity;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity;

public record DeleteRoleCommand(RoleId RoleId) : ICommand;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator(AdminUserQuery adminUserQuery)
    {
        RuleFor(x => x.RoleId).NotEmpty();
        RuleFor(x => x.RoleId).MustAsync(async (rId, ct) =>
            !await adminUserQuery.DoesAdminUserExist(rId, ct)
        ).WithMessage("该角色已分配用户，无法删除");
    }
}

public class DeleteRoleCommandHandler(IRoleRepository roleRepository) : ICommandHandler<DeleteRoleCommand>
{
    public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetAsync(request.RoleId, cancellationToken) ??
                   throw new KnownException($"未找到角色，RoleId = {request.RoleId}");
        await roleRepository.RemoveAsync(role);
    }
}