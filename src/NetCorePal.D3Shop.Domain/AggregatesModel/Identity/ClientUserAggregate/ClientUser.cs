using NetCorePal.D3Shop.Domain.DomainEvents.Identity;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

public partial record ClientUserId : IInt64StronglyTypedId;

public class ClientUser : Entity<ClientUserId>, IAggregateRoot
{
    public ICollection<UserDeliveryAddress> DeliveryAddresses { get; } = [];
    public ICollection<UserThirdPartyLogin> ThirdPartyLogins { get; } = [];

    protected ClientUser()
    {
    }

    public ClientUser(string nickName, string avatar, string phone, string passwordHash, string email)  
    {
        NickName = nickName;
        Avatar = avatar;
        Phone = phone;
        PasswordHash = passwordHash;
        Email = email;
        CreatedAt = DateTime.Now;
        LastLoginAt = DateTime.Now;
    }

    public string NickName { get; private set; } = string.Empty;
    public string Avatar { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime LastLoginAt { get; private set; }
    public bool IsDisabled { get; private set; }
    public DateTime? DisabledTime { get; private set; }
    public string DisabledReason { get; private set; } = string.Empty;
    public int PasswordFailedTimes { get; private set; }
    public bool IsTwoFactorEnabled { get; private set; }

    public void Login()
    {
        // 实现登录逻辑
        AddDomainEvent(new ClientUserLoginEvent(this));
    }

    public void Disable()
    {
        // 实现禁用用户逻辑
    }

    public void EditPassword()
    {
        // 实现修改密码逻辑
    }

    public void ResetPassword()
    {
        // 实现重置密码逻辑
    }
}