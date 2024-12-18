using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using System.ComponentModel.DataAnnotations;

namespace NetCorePal.D3Shop.Admin.Shared.Requests;

public class UpdateDepartmentInfoRequest
{
    [Required] public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Dictionary<AdminUserId, string> Users { get; set; } = [];
}