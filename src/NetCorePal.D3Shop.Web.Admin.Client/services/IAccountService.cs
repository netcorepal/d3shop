using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;


namespace NetCorePal.D3Shop.Web.Admin.Client.Services;

public interface IAccountService
{
    [Post("/api/AdminUserAccount/login")]
    Task<ResponseData> LoginAsync([Body] AdminUserLoginRequest request);
}