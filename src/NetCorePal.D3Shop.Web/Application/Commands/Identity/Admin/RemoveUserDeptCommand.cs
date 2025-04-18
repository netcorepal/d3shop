using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;

/// <summary>
/// 从部门中移除用户命令
/// </summary>
public record RemoveUserDeptCommand(AdminUserId AdminUserId, DeptId DeptId) : ICommand;

/// <summary>
/// 从部门中移除用户命令处理程序
/// </summary>
public class RemoveUserDeptCommandHandler(AdminUserRepository adminUserRepository)
    : ICommandHandler<RemoveUserDeptCommand>
{
    public async Task Handle(RemoveUserDeptCommand request, CancellationToken cancellationToken)
    {
        var user = await adminUserRepository.GetAsync(request.AdminUserId, cancellationToken) ??
                   throw new KnownException($"未找到用户，AdminUserId = {request.AdminUserId}");

        user.RemoveUserDept(request.DeptId);
    }
} 