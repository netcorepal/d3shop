using FluentValidation;
using NetCorePal.D3Shop.Admin.Shared.Dtos.Identity;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;

public record UpdateDepartmrntInfoCommand(
    DeptId DepartmentId,
    string Name,
    string Description,
    IEnumerable<CreateDepartmentUserInfoDto> Users) : ICommand;

public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmrntInfoCommand>
{
    public UpdateDepartmentCommandValidator(DepartmentQuery departmentQuery)
    {
        RuleFor(u => u.Name).NotEmpty().WithMessage("部门名称不能为空");
    }
}

public class UpdateDepartmentInfoCommandHandler(DepartmentRepository departmentRepository)
    : ICommandHandler<UpdateDepartmrntInfoCommand>
{
    public async Task Handle(UpdateDepartmrntInfoCommand request, CancellationToken cancellationToken)
    {
        var department = await departmentRepository.GetAsync(request.DepartmentId, cancellationToken) ??
                         throw new KnownException($"未找到部门，DepartId = {request.DepartmentId}");

        List<DepartmentUser> departmentUsers = [];
        foreach (var user in request.Users) departmentUsers.Add(new DepartmentUser(user.UserName, user.UserId));

        department.UpdateDepartInfo(request.Name, request.Description, departmentUsers);
    }
}