using NetCorePal.D3Shop.Web.Admin.Client.Models;

namespace NetCorePal.D3Shop.Web.Admin.Client.Services
{
    public interface IAccountService
    {
        Task LoginAsync(LoginParamsType model);
        Task<string> GetCaptchaAsync(string? mobile);
    }

    public class AccountService : IAccountService
    {
        private readonly Random _random = new();

        public Task LoginAsync(LoginParamsType model)
        {
            // todo: login logic
            return Task.CompletedTask;
        }

        public Task<string> GetCaptchaAsync(string? mobile)
        {
            var captcha = _random.Next(0, 9999).ToString().PadLeft(4, '0');
            return Task.FromResult(captcha);
        }
    }
}