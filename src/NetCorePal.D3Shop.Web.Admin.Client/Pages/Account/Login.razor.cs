using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Web.Admin.Client.Services;

namespace NetCorePal.D3Shop.Web.Admin.Client.Pages.Account
{
    public partial class Login
    {
        private readonly LoginParamsType _model = new();

        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        [Inject] public IAccountService AccountService { get; set; } = default!;

        [Inject] public MessageService Message { get; set; } = default!;

        public async Task HandleSubmit()
        {
            var request = new AdminUserLoginRequest(_model.Name, _model.Password);
            var response = await AccountService.LoginAsync(request);
            if (response.Success)
                NavigationManager.NavigateTo("/admin", forceLoad: true);
            else
                await Message.Error(response.Message);
        }

        public async Task GetCaptcha()
        {
            await Message.Success($"Verification code validated successfully! The verification code is: ");
        }
    }

    public class LoginParamsType
    {
        [Required] public string Name { get; set; } = string.Empty;

        [Required] public string Password { get; set; } = string.Empty;

        public string? Mobile { get; set; }

        public string? Captcha { get; set; }

        public string? LoginType { get; set; }

        public bool AutoLogin { get; set; }
    }
}