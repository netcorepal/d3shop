using NetCorePal.Extensions.Dto;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.VueAdmin.Requests
{
    public class VueRoleQueryRequest : PageRequest
    {
        public string? Name { get; set; }
        public string? Remark { get; set; }

        public int? Status { get; set; }
    }
}
