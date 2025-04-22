using System.Security.Claims;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

namespace NetCorePal.D3Shop.Web.Auth;

public interface ICurrentUser<out TUserId>
{
    TUserId UserId { get; }
    bool IsAuthenticated { get; }
    string? GetClaimValue(string claimType);
}

public interface ICurrentClientUser : ICurrentUser<ClientUserId>;

public class CurrentClientUser(IHttpContextAccessor httpContextAccessor) : ICurrentClientUser
{
    public ClientUserId UserId
    {
        get
        {
            var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? throw new InvalidOperationException("User is not authenticated.");
            return new ClientUserId(long.Parse(userId));
        }
    }

    public bool IsAuthenticated =>
        httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public string? GetClaimValue(string claimType)
    {
        return httpContextAccessor.HttpContext?.User?.FindFirstValue(claimType);
    }
}




public interface ICurrentVueAdminUser : ICurrentUser<AdminUserId>;

public class CurrentVueAdminUser(IHttpContextAccessor httpContextAccessor) : ICurrentVueAdminUser
{
    public AdminUserId UserId
    {
        get
        {
            var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? throw new InvalidOperationException("User is not authenticated.");
            return new AdminUserId(long.Parse(userId));
        }
    }

    public bool IsAuthenticated =>
        httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public string? GetClaimValue(string claimType)
    {
        return httpContextAccessor.HttpContext?.User?.FindFirstValue(claimType);
    }
}