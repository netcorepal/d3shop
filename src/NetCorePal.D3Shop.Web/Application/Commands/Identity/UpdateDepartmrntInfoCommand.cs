using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity;

public record UpdateDepartmrntInfoCommand(DeptId DepartmentId, string Name, string Description, Dictionary<AdminUserId, string> Users) : ICommand;

public class UpdateDepartmentInfoCommandHandler(DepartmentRepository departmentRepository)
    : ICommandHandler<UpdateDepartmrntInfoCommand>
{
    public async Task Handle(UpdateDepartmrntInfoCommand request, CancellationToken cancellationToken)
    {
        var department = await departmentRepository.GetAsync(request.DepartmentId, cancellationToken) ??
                   throw new KnownException($"未找到部门，DepartId = {request.DepartmentId}");

        List<DepartmentUser> departmentUsers = [];
        foreach (var user in request.Users)
        {
            departmentUsers.Add(new DepartmentUser(user.Value, user.Key));
        }

        department.UpdateDepartInfo(request.Name, request.Description, departmentUsers);
    }
}