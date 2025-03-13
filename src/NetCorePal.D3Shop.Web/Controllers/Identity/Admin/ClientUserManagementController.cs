using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Admin.Shared.Permission;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;
using NetCorePal.D3Shop.Web.Auth;
using NetCorePal.D3Shop.Web.Blazor;
using NetCorePal.D3Shop.Web.Controllers.Identity.Client.Requests;
using NetCorePal.Extensions.Dto;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Admin;

[Route("api/[controller]/[action]")]
[ApiController]
[KnownExceptionHandler]
[AdminPermission(PermissionCodes.AdminUserManagement)]
public class ClientUserManagementController(
    IMediator mediator) : ControllerBase
{
    [HttpPut]
    public async Task<ResponseData> Disable([FromBody] ClientUserDisableRequest request)
    {
        return await mediator.Send(new DisableClientUserCommand(
            request.UserId,
            request.Reason
        )).AsResponseData();
    }

    [HttpPut]
    public async Task<ResponseData> Enable([FromBody] ClientUserId request)
    {
        return await mediator.Send(new EnableClientUserCommand(
            request
        )).AsResponseData();
    }
}