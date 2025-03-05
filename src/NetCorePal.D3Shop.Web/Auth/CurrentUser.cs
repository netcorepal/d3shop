using System.Security.Claims;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

namespace NetCorePal.D3Shop.Web.Auth;

public interface ICurrentUser<out TUserId>
{
    TUserId UserId { get; }
    bool IsAuthenticated { get; }
    string? GetClaimValue(string claimType);
}

public class ClientCurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser<ClientUserId>
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