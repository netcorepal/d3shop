using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;

/// <summary>
/// 添加用户到部门命令
/// </summary>
public record AddUserDeptCommand(AdminUserId AdminUserId, DeptId DeptId, string DeptName) : ICommand;

/// <summary>
/// 添加用户到部门命令处理程序
/// </summary>
public class AddUserDeptCommandHandler(AdminUserRepository adminUserRepository)
    : ICommandHandler<AddUserDeptCommand>
{
    public async Task Handle(AddUserDeptCommand request, CancellationToken cancellationToken)
    {
        var user = await adminUserRepository.GetAsync(request.AdminUserId, cancellationToken) ??
                   throw new KnownException($"未找到用户，AdminUserId = {request.AdminUserId}");

        user.AddUserDept(request.DeptId, request.DeptName);
    }
} 