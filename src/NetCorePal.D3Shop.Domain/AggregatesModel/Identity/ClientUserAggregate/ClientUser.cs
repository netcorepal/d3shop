using NetCorePal.D3Shop.Domain.DomainEvents.Identity.Client;
using NetCorePal.Extensions.Domain;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

public partial record ClientUserId : IInt64StronglyTypedId;

public class ClientUser : Entity<ClientUserId>, IAggregateRoot
{
    protected ClientUser()
    {
    }

    public ClientUser(
        string nickName,
        string avatar,
        string phone,
        string passwordHash,
        string passwordSalt,
        string email)
    {
        NickName = nickName;
        Avatar = avatar;
        Phone = phone;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        Email = email;
        CreatedAt = DateTime.Now;
        LastLoginAt = DateTime.Now;
    }

    public ICollection<UserDeliveryAddress> DeliveryAddresses { get; } = [];
    public ICollection<UserThirdPartyLogin> ThirdPartyLogins { get; } = [];

    public string NickName { get; private set; } = string.Empty;
    public string Avatar { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string PasswordSalt { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime LastLoginAt { get; private set; }
    public bool IsDisabled { get; private set; }
    public DateTime? DisabledTime { get; private set; }
    public string DisabledReason { get; private set; } = string.Empty;
    public int PasswordFailedTimes { get; private set; }
    public bool IsTwoFactorEnabled { get; private set; }

    /// <summary>
    ///     用户登录
    /// </summary>
    /// <param name="passwordHash"></param>
    /// <param name="loginTime"></param>
    /// <param name="loginMethod"></param>
    /// <param name="ipAddress"></param>
    /// <param name="userAgent"></param>
    public void Login(
        string passwordHash,
        DateTime loginTime,
        string loginMethod,
        string ipAddress,
        string userAgent)
    {
        if (PasswordHash != passwordHash)
        {
            PasswordFailedTimes++;
            throw new KnownException("用户名或密码错误");
        }

        PasswordFailedTimes = 0;
        LastLoginAt = loginTime;
        AddDomainEvent(new ClientUserLoginEvent(Id, NickName, loginTime, loginMethod, ipAddress, userAgent));
    }

    /// <summary>
    ///     禁用用户
    /// </summary>
    /// <param name="reason"></param>
    /// <exception cref="KnownException"></exception>
    public void Disable(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new KnownException("禁用原因不能为空");

        if (IsDisabled) return;
        IsDisabled = true;
        DisabledTime = DateTime.UtcNow;
        DisabledReason = reason.Trim();
    }

    /// <summary>
    ///     启用用户
    /// </summary>
    public void Enable()
    {
        if (!IsDisabled) return;
        IsDisabled = false;
        DisabledTime = null;
        DisabledReason = string.Empty;
    }

    /// <summary>
    ///     修改密码
    /// </summary>
    /// <param name="oldPasswordHash"></param>
    /// <param name="newPasswordHash"></param>
    public void EditPassword(string oldPasswordHash, string newPasswordHash)
    {
        if (PasswordHash != oldPasswordHash) throw new KnownException("旧密码不正确");
        PasswordHash = newPasswordHash;
    }

    /// <summary>
    ///     重置密码
    /// </summary>
    /// <param name="newPasswordHash"></param>
    public void ResetPassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
    }

    /// <summary>
    ///     新增收货地址
    /// </summary>
    /// <param name="address"></param>
    /// <param name="recipientName"></param>
    /// <param name="phone"></param>
    /// <param name="setAsDefault"></param>
    public void AddDeliveryAddress(
        string address,
        string recipientName,
        string phone,
        bool setAsDefault)
    {
        var newAddress = new UserDeliveryAddress(
            Id,
            address,
            recipientName,
            phone,
            setAsDefault
        );

        if (setAsDefault)
        {
            // 确保只有一个默认地址
            var addr = DeliveryAddresses.SingleOrDefault(a => a.IsDefault);
            addr?.UnsetDefault();
        }

        DeliveryAddresses.Add(newAddress);
    }

    /// <summary>
    ///     更新收货地址
    /// </summary>
    /// <param name="deliveryAddressId"></param>
    /// <param name="address"></param>
    /// <param name="recipientName"></param>
    /// <param name="phone"></param>
    /// <param name="setAsDefault"></param>
    /// <exception cref="KnownException"></exception>
    public void UpdateDeliveryAddress(
        DeliveryAddressId deliveryAddressId,
        string address,
        string recipientName,
        string phone,
        bool setAsDefault)
    {
        var deliveryAddress = DeliveryAddresses.SingleOrDefault(a => a.Id == deliveryAddressId) ??
                              throw new KnownException("地址不存在");
        deliveryAddress.UpdateDetails(address, recipientName, phone);

        if (!setAsDefault) return;

        var addr = DeliveryAddresses
            .SingleOrDefault(a => a.IsDefault && a.Id != deliveryAddressId);
        addr?.UnsetDefault();

        if (!deliveryAddress.IsDefault)
            deliveryAddress.SetAsDefault();
    }

    /// <summary>
    ///     删除收货地址
    /// </summary>
    /// <param name="deliveryAddressId"></param>
    /// <exception cref="KnownException"></exception>
    public void RemoveDeliveryAddress(DeliveryAddressId deliveryAddressId)
    {
        var deliveryAddress = DeliveryAddresses.SingleOrDefault(a => a.Id == deliveryAddressId) ??
                              throw new KnownException("地址不存在");
        DeliveryAddresses.Remove(deliveryAddress);
    }

    /// <summary>
    ///     绑定第三方登录
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="appId"></param>
    /// <param name="openId"></param>
    /// <exception cref="KnownException"></exception>
    public void BindThirdPartyLogin(
        ThirdPartyProvider provider,
        string appId,
        string openId)
    {
        // 规则：同一提供商下不允许重复绑定
        if (ThirdPartyLogins.Any(x => x.Provider == provider && x.AppId == appId))
            throw new KnownException("该渠道已绑定，请勿重复操作");

        var login = new UserThirdPartyLogin(
            Id,
            provider,
            appId,
            openId
        );

        ThirdPartyLogins.Add(login);
    }

    /// <summary>
    ///     解绑第三方登录
    /// </summary>
    /// <param name="loginId"></param>
    /// <exception cref="KnownException"></exception>
    public void UnbindThirdPartyLogin(ThirdPartyLoginId loginId)
    {
        var login = ThirdPartyLogins.SingleOrDefault(x => x.Id == loginId);
        if (login == null) return;

        // 规则：至少保留一种登录方式
        if (ThirdPartyLogins.Count == 1)
            throw new KnownException("无法解绑最后一种登录方式");

        ThirdPartyLogins.Remove(login);
    }
}