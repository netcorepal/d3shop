using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;

namespace NetCorePal.D3Shop.Admin.Shared.Responses;

public class AdminUserResponse(AdminUserId id, string name, string phone, IEnumerable<string> roles,string realName)
{
    public AdminUserId Id { get; } = id;
    public string Name { get; set; } = name;
    public string Phone { get; set; } = phone;
    public IEnumerable<string> Roles { get; set; } = roles;
    public string RealName { get; set; } = realName;


    /// <summary>
    /// 这里确认是使用name还是userName
    /// </summary>
    public string UserName { get; set; } = name;
}
