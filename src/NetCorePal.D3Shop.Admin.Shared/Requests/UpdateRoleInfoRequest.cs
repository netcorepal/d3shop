using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using System.ComponentModel.DataAnnotations;

namespace NetCorePal.D3Shop.Admin.Shared.Requests;

public class UpdateRoleInfoRequest
{
    [Required] public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int Status { get; set; }


    public IEnumerable<MenuId> Permissions { get; set; } = [];
}