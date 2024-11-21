using NetCorePal.D3Shop.Web.Admin.Client.Attributes;

namespace NetCorePal.D3Shop.Web.Admin.Client.Services;

[RefitService]
public interface IAccountService
{
    [Post("/api/AdminUserAccount/login")]
    Task<ResponseData> LoginAsync([Body] AdminUserLoginRequest request);
}