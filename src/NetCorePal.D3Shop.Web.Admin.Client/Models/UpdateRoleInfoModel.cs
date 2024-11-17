using System.ComponentModel.DataAnnotations;

namespace NetCorePal.D3Shop.Web.Admin.Client.Models;

public class UpdateRoleInfoModel
{
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Description { get; set; } = string.Empty;
}