using NetCorePal.Extensions.Dto;

namespace NetCorePal.D3Shop.Admin.Shared.Requests;

public class DepartmentQueryRequest : PageRequest
{
    public string? Name { get; set; }
}