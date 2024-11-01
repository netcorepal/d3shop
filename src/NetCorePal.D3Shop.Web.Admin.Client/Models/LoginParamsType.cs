using System.ComponentModel.DataAnnotations;

namespace NetCorePal.D3Shop.Web.Admin.Client.Models
{
    public class LoginParamsType
    {
        [Required] public string? Name { get; set; }

        [Required] public string? Password { get; set; }

        public string? Mobile { get; set; }

        public string? Captcha { get; set; }

        public string? LoginType { get; set; }

        public bool AutoLogin { get; set; }
    }
}