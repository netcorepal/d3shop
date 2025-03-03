using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Client;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Client.Dto;
using NetCorePal.D3Shop.Web.Auth;
using NetCorePal.D3Shop.Web.Controllers.Identity.Client.Requests;
using NetCorePal.D3Shop.Web.Helper;
using NetCorePal.Extensions.Dto;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client;

[Route("api/[controller]/[action]")]
[ApiController]
[ClientAuthorize]
public class ClientUserController(
    IMediator mediator,
    ClientUserQuery clientUserQuery) : ControllerBase
{
    [HttpPost]
    public async Task<ResponseData> AddDeliveryAddress([FromBody] AddDeliveryAddressRequest request)
    {
        return await mediator.Send(new ClientUserAddDeliveryAddressCommand(
            request.UserId,
            request.Address,
            request.RecipientName,
            request.Phone,
            request.SetAsDefault
        )).AsResponseData();
    }

    [HttpGet]
    public async Task<ResponseData<List<ClientUserDeliveryAddressInfo>>> GetDeliveryAddresses(
        [FromQuery] ClientUserId userId)
    {
        var addresses = await clientUserQuery.GetDeliveryAddressesAsync(userId);
        return addresses.AsResponseData();
    }

    [HttpDelete]
    public async Task<ResponseData> RemoveDeliveryAddress([FromQuery] RemoveDeliveryAddressRequest request)
    {
        return await mediator.Send(new ClientUserRemoveDeliveryAddressCommand(
            request.UserId,
            request.DeliveryAddressId
        )).AsResponseData();
    }

    [HttpPut]
    public async Task<ResponseData> UpdateDeliveryAddress([FromBody] UpdateDeliveryAddressRequest request)
    {
        return await mediator.Send(new ClientUserUpdateDeliveryAddressCommand(
            request.UserId,
            request.DeliveryAddressId,
            request.Address,
            request.RecipientName,
            request.Phone,
            request.SetAsDefault
        )).AsResponseData();
    }

    [HttpPost]
    public async Task<ResponseData<ThirdPartyLoginId>> BindThirdPartyLogin(
        [FromBody] BindThirdPartyLoginRequest request)
    {
        return await mediator.Send(new ClientUserBindThirdPartyLoginCommand(
            request.UserId,
            request.ThirdPartyProvider,
            request.AppId,
            request.OpenId
        )).AsResponseData();
    }

    [HttpGet]
    public async Task<ResponseData<List<ClientUserThirdPartyLoginInfo>>> GetThirdPartyLogins(
        [FromQuery] ClientUserId userId)
    {
        var thirdPartyLogins = await clientUserQuery.GetThirdPartyLoginsAsync(userId);
        return thirdPartyLogins.AsResponseData();
    }

    [HttpDelete]
    public async Task<ResponseData> UnbindThirdPartyLogin([FromQuery] UnbindThirdPartyLoginRequest request)
    {
        return await mediator.Send(new ClientUserUnbindThirdPartyLoginCommand(
            request.UserId,
            request.ThirdPartyLoginId
        )).AsResponseData();
    }

    [HttpPut]
    public async Task<ResponseData> EditPassword([FromBody] EditPasswordRequest request)
    {
        var salt = await clientUserQuery.GetUserPasswordSaltByIdAsync(request.UserId);
        var oldPasswordHash = NewPasswordHasher.HashPassword(request.OldPassword, salt);
        var newPasswordHash = NewPasswordHasher.HashPassword(request.NewPassword, salt);

        return await mediator.Send(new ClientUserEditPasswordCommand(
            request.UserId,
            oldPasswordHash,
            newPasswordHash
        )).AsResponseData();
    }

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