namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate.Dto
{
    public class AddUserPermissionDto
    {
        public string PermissionCode { get; set; } = string.Empty;
        public string PermissionRemark { get; set; } = string.Empty;
    }
}
