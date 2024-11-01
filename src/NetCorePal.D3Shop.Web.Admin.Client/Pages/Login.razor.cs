using Microsoft.AspNetCore.Components;
using NetCorePal.D3Shop.Web.Admin.Client.Models;
using NetCorePal.D3Shop.Web.Admin.Client.Services;

namespace NetCorePal.D3Shop.Web.Admin.Client.Pages
{
    public partial class Login
    {
        private readonly LoginParamsType _model = new();

        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        [Inject] public IAccountService AccountService { get; set; } = default!;

        [Inject] public MessageService Message { get; set; } = default!;

        public void HandleSubmit()
        {
            switch (_model)
            {
                case { Name: "admin", Password: "ant.design" }:
                    NavigationManager.NavigateTo("/");
                    return;
                case { Name: "user", Password: "ant.design" }:
                    NavigationManager.NavigateTo("/");
                    break;
            }
        }

        public async Task GetCaptcha()
        {
            var captcha = await AccountService.GetCaptchaAsync(_model.Mobile);
            await Message.Success($"Verification code validated successfully! The verification code is: {captcha}");
        }
    }
}