using System.ComponentModel.DataAnnotations;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Admin.Shared.Requests;

public class CreateAdminUserRequest
{
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Phone { get; set; } = string.Empty;
    [Required] public string PassWord { get; set; } = string.Empty;
    public IEnumerable<RoleId> RoleIds { get; set; } = [];
}