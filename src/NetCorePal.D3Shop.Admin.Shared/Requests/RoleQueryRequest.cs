using NetCorePal.Extensions.Dto;

namespace NetCorePal.D3Shop.Admin.Shared.Requests;

public class RoleQueryRequest : PageRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public int? Status { get; set; }
}