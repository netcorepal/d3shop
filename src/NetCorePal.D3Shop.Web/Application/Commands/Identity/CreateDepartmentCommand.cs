using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Dto;
using NetCorePal.D3Shop.Web.Application.Queries.Identity;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity;

public record CreateDepartmentCommand(
    string Name,
    string Description,
    DeptId? ParentId)
    : ICommand<DeptId>;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator(DepartmentQuery departmentQuery)
    {
        RuleFor(u => u.Name).NotEmpty().WithMessage("部门名称不能为空");
        //RuleFor(u => u.Description).NotEmpty().WithMessage("不能为空");
        RuleFor(u => u.Name).MustAsync(async (n, ct) => !await departmentQuery.DoesDepartmentExist(n, ct))
            .WithMessage(u => $"该部门已存在，Name={u.Name}");
    }
}

public class CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository)
    : ICommandHandler<CreateDepartmentCommand, DeptId>
{
    public async Task<DeptId> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
     
        var department = new Department(request.Name, request.Description, request.ParentId);

        await departmentRepository.AddAsync(department, cancellationToken);
        return department.Id;
    }
}