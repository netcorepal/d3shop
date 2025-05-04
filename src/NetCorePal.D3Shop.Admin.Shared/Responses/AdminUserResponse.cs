using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Admin.Shared.Responses;

public class AdminUserResponse(AdminUserId id, string name, string phone, IEnumerable<string> roles, string realName,int status,string email, DateTimeOffset createdAt)
{
    public AdminUserId Id { get; } = id;
    public string Name { get; set; } = name;
    public string Phone { get; set; } = phone;
    public IEnumerable<string> Roles { get; set; } = roles;

    public IEnumerable<string> RoleIds { get; set; } = roles;

    public string RealName { get; set; } = realName;

    public int Status { get; set; } = status;

    public DateTimeOffset CreatedAt { get; set; } = createdAt;

    public string Email { get; private set; } = email;
}
