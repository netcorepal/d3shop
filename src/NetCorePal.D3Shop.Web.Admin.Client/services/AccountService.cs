using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Web.Admin.Client.Pages.Account;

namespace NetCorePal.D3Shop.Web.Admin.Client.Services
{
    public class AccountService(
        ApiHttpClient httpClient,
        MessageService message)
    {
        private readonly Random _random = new();

        public async Task<bool> LoginAsync(LoginParamsType model)
        {
            var request = new AdminUserLoginRequest(model.Name, model.Password);
            var response = await httpClient.PostWithDataAsync<AminUserTokenResponse, AdminUserLoginRequest>(
                "/api/AdminUserToken/login", request);
            if (response.Success)
                return true;
            else
                await message.Error(response.Message);

            return false;
        }

        public Task<string> GetCaptchaAsync(string? mobile)
        {
            var captcha = _random.Next(0, 9999).ToString().PadLeft(4, '0');
            return Task.FromResult(captcha);
        }
    }
}