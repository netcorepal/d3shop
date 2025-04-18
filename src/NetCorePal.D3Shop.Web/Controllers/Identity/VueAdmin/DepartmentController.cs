using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.VueAdmin;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.D3Shop.Web.Controllers.Identity.VueAdmin.Requests;
using NetCorePal.D3Shop.Web.Controllers.Identity.VueAdmin.Responses;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;
using NetCorePal.D3Shop.Web.Auth;
using NetCorePal.D3Shop.Web.Blazor;
using NetCorePal.D3Shop.Admin.Shared.Permission;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;
using NetCorePal.Extensions.AspNetCore;
using NetCorePal.D3Shop.Admin.Shared.Responses;

namespace PlaygroundApi.Controllers
{
    [Route("api/system/dept")]
    [ApiController]
    [AdminPermission(PermissionCodes.DepartmentManagement)]
    public class DepartmentController(IMediator mediator, DepartmentQuery departmentQuery) : ControllerBase
    {
        private CancellationToken CancellationToken => HttpContext?.RequestAborted ?? default;

        [HttpPost]
        [AdminPermission(PermissionCodes.DepartmentCreate)]
        public async Task<ResponseData<DeptId>> CreateDepartment([FromBody] CreateDepartmentRequest request)
        {
            var departmentId = await mediator.Send(new CreateDepartmentCommand(
                request.Name,
                request.Remark,
                request.Users,
                request.Pid,
                request.Status
                ),
                CancellationToken);

            return new ResponseData<DeptId>(departmentId);
        }

        [HttpGet("list")]
        public async Task<ResponseData<List<DepartmentResponse>>> GetDepartments([FromQuery] DepartmentQueryRequest request)
        {
            var departments = await departmentQuery.GetAllDepartmentsAsync(request, CancellationToken);
            return departments.AsResponseData();
        }

        [HttpGet("{id}")]
        public async Task<ResponseData<DepartmentResponse>> GetDepartment(DeptId id)
        {
            var department = await departmentQuery.GetDeptByIdAsync(id, CancellationToken);
            if (department == null)
            {
                throw new KnownException("部门不存在", -1);
            }

            return department.AsResponseData();
        }



        [HttpPut("{id}")]
        public async Task<ResponseData> UpdateDepartment([FromRoute] DeptId id, [FromBody] UpdateDepartmentInfoRequest request)
        {
            await mediator.Send(new UpdateDepartmrntInfoCommand(
                id,
                request.Name,
                request.Remark,
                request.Code,
                request.ParentId,
                request.Status,
                request.Users
               ),
                CancellationToken);

            return new ResponseData();
        }

        [HttpPut("{id}/status")]
        public async Task<ResponseData<DeptId>> UpdateDepartmentStatus(DeptId id, [FromBody] VueUpdateDepartmentStatusRequest request)
        {
            var command = new VueUpdateDepartmentStatusCommand(
                id,
                request.Status);

            await mediator.Send(command);
            return id.AsResponseData();
        }

        [HttpDelete("{id}")]
        public async Task<ResponseData> DeleteDepartment([FromRoute] DeptId id)
        {
            await mediator.Send(new DeleteDepartmentCommand(id), CancellationToken);
            return new ResponseData();
        }
    }
}

