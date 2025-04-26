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
    string Remark,
    string Code,
    DeptId ParentId,
    int Status) : ICommand;

public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmrntInfoCommand>
{
    public UpdateDepartmentCommandValidator(DepartmentQuery departmentQuery)
    {
        //RuleFor(x => x.DepartmentId).Must(x => x.Id == 0).WithMessage("部门ID不能为空");
        RuleFor(x => x.Name).NotEmpty().WithMessage("部门名称不能为空");
        RuleFor(x => x.Status).InclusiveBetween(0, 1).WithMessage("状态值必须为0或1");
    }
}

public class UpdateDepartmentInfoCommandHandler(DepartmentRepository departmentRepository)
    : ICommandHandler<UpdateDepartmrntInfoCommand>
{
    public async Task Handle(UpdateDepartmrntInfoCommand request, CancellationToken cancellationToken)
    {
        var department = await departmentRepository.GetAsync(request.DepartmentId, cancellationToken) ??
                         throw new KnownException($"未找到部门，DepartId = {request.DepartmentId}");

        department.UpdateDepartInfo(
                request.Name,
                request.Code,
                request.Remark,
                request.Status);
    }
}