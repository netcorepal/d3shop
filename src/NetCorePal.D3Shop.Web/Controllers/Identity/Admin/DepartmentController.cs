using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Admin.Shared.Permission;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Web.Admin.Client.Services;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.VueAdmin;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.D3Shop.Web.Auth;
using NetCorePal.D3Shop.Web.Blazor;
using NetCorePal.D3Shop.Web.Controllers.Identity.VueAdmin.Requests;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Admin;

[Route("api/[controller]/[action]")]
[ApiController]
[KnownExceptionHandler]
[AdminPermission(PermissionCodes.DepartmentManagement)]
public class DepartmentController(
    IMediator mediator,
    DepartmentQuery departmentQuery)
    : ControllerBase, IDepartmentService
{
    private CancellationToken CancellationToken => HttpContext?.RequestAborted ?? default;

    [HttpPost]
    [AdminPermission(PermissionCodes.DepartmentCreate)]
    public async Task<ResponseData<DeptId>> CreateDepartment([FromBody] CreateDepartmentRequest request)
    {
        var departmentId = await mediator.Send(
            new CreateDepartmentCommand(
                request.Name,
                request.Remark,
                request.Pid,
                request.Status
                ),
            CancellationToken);

        return departmentId.AsResponseData();
    }


    [HttpGet]
    public async Task<ResponseData<List<DepartmentResponse>>> GetAllDepartments(
        [FromQuery] DepartmentQueryRequest request)
    {
        var department = await departmentQuery.GetAllDepartmentsAsync(request, CancellationToken);
        return department.AsResponseData();
    }


    [HttpPut("{id}")]
    [AdminPermission(PermissionCodes.DepartmentEdit)]
    public async Task<ResponseData> UpdateDepartmentInfo([FromRoute] DeptId id,
        [FromBody] UpdateDepartmentInfoRequest request)
    {
        await mediator.Send(new UpdateDepartmrntInfoCommand(
                 id,
                 request.Name,
                 request.Remark,
                 request.Code,
                 request.ParentId,
                 request.Status
                ),
                 CancellationToken);
        return new ResponseData();
    }


    [HttpDelete("{id}")]
    [AdminPermission(PermissionCodes.DepartmentDelete)]
    public async Task<ResponseData> DeleteDepartment([FromRoute] DeptId id)
    {
        await mediator.Send(new DeleteDepartmentCommand(id), CancellationToken);
        return new ResponseData();
    }

    /// <summary>
    /// 添加用户到部门
    /// </summary>
    [HttpPost]
    [AdminPermission(PermissionCodes.DepartmentEdit)]
    public async Task<ResponseData> AddUserToDepartment([FromBody] AddUserDeptRequest request)
    {
        await mediator.Send(new AddUserDeptCommand(
                request.AdminUserId,
                request.DeptId,
                request.DeptName
            ),
            CancellationToken);
        return new ResponseData();
    }

    /// <summary>
    /// 从部门中移除用户
    /// </summary>
    [HttpPost]
    [AdminPermission(PermissionCodes.DepartmentEdit)]
    public async Task<ResponseData> RemoveUserFromDepartment([FromBody] RemoveUserDeptRequest request)
    {
        await mediator.Send(new RemoveUserDeptCommand(
                request.AdminUserId,
                request.DeptId
            ),
            CancellationToken);
        return new ResponseData();
    }

    //[HttpPut("{id}/status")]
    //public async Task<ResponseData<DeptId>> UpdateDepartmentStatus(DeptId id, [FromBody] VueUpdateDepartmentStatusRequest request)
    //{
    //    var command = new VueUpdateDepartmentStatusCommand(
    //        id,
    //        request.Status);

    //    await mediator.Send(command);
    //    return id.AsResponseData();
    //}


    //[HttpGet("{id}")]
    //public async Task<ResponseData<DepartmentResponse>> GetDepartment(DeptId id)
    //{
    //    var department = await departmentQuery.GetDeptByIdAsync(id, CancellationToken);
    //    if (department == null)
    //    {
    //        throw new KnownException("部门不存在", -1);
    //    }

    //    return department.AsResponseData();
    //}
}