using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;

public record DeleteDepartmentCommand(DeptId DeptId) : ICommand;

public class DeleteDepartmentCommandHandler(IDepartmentRepository departmentRepository)
    : ICommandHandler<DeleteDepartmentCommand>
{
    public async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var depart = await departmentRepository.GetAsync(request.DeptId, cancellationToken) ??
                     throw new KnownException($"部门不存在，DeptId={request.DeptId}");


        depart.Delete();
    }
}