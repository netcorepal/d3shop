using System.ComponentModel.DataAnnotations;

namespace NetCorePal.D3Shop.Admin.Shared.Requests;

public class AdminUserLoginRequest
{
    [Required] public string Name { get; set; } = string.Empty;

    [Required] public string Password { get; set; } = string.Empty;

    public bool IsPersistent { get; set; }
}