using FluentValidation;
using NetCorePal.D3Shop.Admin.Shared.Dtos.Identity;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;

/// <summary>
/// 修改部门用户数量
/// </summary>
/// <param name="DepartmentId"></param>
/// <param name="UserCount"></param>
public record UpdateDeptUserCountCommand(DeptId DepartmentId, int UserCount) : ICommand;


public class UpdateDeptUserCommandHandler(DepartmentRepository departmentRepository) : ICommandHandler<UpdateDeptUserCountCommand>
{
    public async Task Handle(UpdateDeptUserCountCommand request, CancellationToken cancellationToken)
    {
        var department = await departmentRepository.GetAsync(request.DepartmentId, cancellationToken) ??
                         throw new KnownException($"未找到部门，DepartId = {request.DepartmentId}");
        department.SetUserCount(request.UserCount);
    }
}