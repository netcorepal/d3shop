using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;


namespace NetCorePal.D3Shop.Web.Admin.Client.Services;

public interface IAccountService
{
    [Post("/api/AdminUserToken/login")]
    Task<ResponseData<AdminUserTokenResponse>> LoginAsync([Body] AdminUserLoginRequest request);
}