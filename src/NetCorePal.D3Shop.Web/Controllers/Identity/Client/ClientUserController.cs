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
    ClientUserQuery clientUserQuery,
    ICurrentClientUser currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<ResponseData<ClientUserInfo>> GetClientUserInfo()
    {
        var userInfo = await clientUserQuery.GetClientUserInfoByIdAsync(currentUser.UserId, HttpContext.RequestAborted);
        return userInfo.AsResponseData();
    }

    [HttpPost]
    public async Task<ResponseData> AddDeliveryAddress([FromBody] AddDeliveryAddressRequest request)
    {
        return await mediator.Send(new ClientUserAddDeliveryAddressCommand(
            currentUser.UserId,
            request.Province,
            request.ProvinceCode,
            request.City,
            request.CityCode,
            request.District,
            request.DistrictCode,
            request.Address,
            request.RecipientName,
            request.Phone,
            request.SetAsDefault
        )).AsResponseData();
    }

    [HttpGet]
    public async Task<ResponseData<List<ClientUserDeliveryAddressInfo>>> GetDeliveryAddresses()
    {
        var addresses = await clientUserQuery.GetDeliveryAddressesAsync(currentUser.UserId, HttpContext.RequestAborted);
        return addresses.AsResponseData();
    }

    [HttpDelete]
    public async Task<ResponseData> RemoveDeliveryAddress(DeliveryAddressId deliveryAddressId)
    {
        return await mediator.Send(new ClientUserRemoveDeliveryAddressCommand(
            currentUser.UserId,
            deliveryAddressId
        )).AsResponseData();
    }

    [HttpPut]
    public async Task<ResponseData> UpdateDeliveryAddress([FromBody] UpdateDeliveryAddressRequest request)
    {
        return await mediator.Send(new ClientUserUpdateDeliveryAddressCommand(
            currentUser.UserId,
            request.DeliveryAddressId,
            request.Province,
            request.ProvinceCode,
            request.City,
            request.CityCode,
            request.District,
            request.DistrictCode,
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
            currentUser.UserId,
            request.ThirdPartyProvider,
            request.AppId,
            request.OpenId
        )).AsResponseData();
    }

    [HttpGet]
    public async Task<ResponseData<List<ClientUserThirdPartyLoginInfo>>> GetThirdPartyLogins()
    {
        var thirdPartyLogins =
            await clientUserQuery.GetThirdPartyLoginsAsync(currentUser.UserId, HttpContext.RequestAborted);
        return thirdPartyLogins.AsResponseData();
    }

    [HttpDelete]
    public async Task<ResponseData> UnbindThirdPartyLogin(ThirdPartyLoginId thirdPartyLoginId)
    {
        return await mediator.Send(new ClientUserUnbindThirdPartyLoginCommand(
            currentUser.UserId,
            thirdPartyLoginId
        )).AsResponseData();
    }

    [HttpPut]
    public async Task<ResponseData> EditPassword([FromBody] EditPasswordRequest request)
    {
        var userId = currentUser.UserId;
        var salt = await clientUserQuery.GetUserPasswordSaltByIdAsync(userId, HttpContext.RequestAborted);
        var oldPasswordHash = NewPasswordHasher.HashPassword(request.OldPassword, salt);
        var newPasswordHash = NewPasswordHasher.HashPassword(request.NewPassword, salt);

        return await mediator.Send(new ClientUserEditPasswordCommand(
            userId,
            oldPasswordHash,
            newPasswordHash
        )).AsResponseData();
    }
}