using NetCorePal.Extensions.Dto;

namespace NetCorePal.D3Shop.Admin.Shared.Requests;

public class AdminUserQueryRequest : PageRequest
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
}