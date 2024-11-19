using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Web.Admin.Client;
using NetCorePal.D3Shop.Web.Application.Queries.Identity;

namespace NetCorePal.D3Shop.Web.Blazor.Components
{
    // This is a server-side AuthenticationStateProvider that uses PersistentComponentState to flow the
    // authentication state to the client which is then fixed for the lifetime of the WebAssembly application.
    internal sealed class PersistingServerAuthenticationStateProvider : ServerAuthenticationStateProvider, IDisposable
    {
        private readonly PersistentComponentState _state;
        private readonly IdentityOptions _options;
        private readonly AdminUserQuery _adminUserQuery;

        private readonly PersistingComponentStateSubscription _subscription;

        private Task<AuthenticationState>? _authenticationStateTask;

        public PersistingServerAuthenticationStateProvider(
            PersistentComponentState persistentComponentState,
            IOptions<IdentityOptions> optionsAccessor,
            AdminUserQuery adminUserQuery)
        {
            _state = persistentComponentState;
            _options = optionsAccessor.Value;
            _adminUserQuery = adminUserQuery;

            AuthenticationStateChanged += OnAuthenticationStateChanged;
            _subscription = _state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
        }

        private void OnAuthenticationStateChanged(Task<AuthenticationState> task)
        {
            _authenticationStateTask = task;
        }

        private async Task OnPersistingAsync()
        {
            if (_authenticationStateTask is null)
            {
                throw new UnreachableException($"Authentication state not set in {nameof(OnPersistingAsync)}().");
            }

            var authenticationState = await _authenticationStateTask;
            var principal = authenticationState.User;

            if (principal.Identity?.IsAuthenticated == true)
            {
                var userIdString = principal.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;
                if (userIdString != null)
                {
                    if (!long.TryParse(userIdString, out var userId))
                        throw new InvalidOperationException("User Id could not be parsed to a valid long value.");
                    var permissions = await _adminUserQuery.GetAdminUserPermissionCodes(new AdminUserId(userId));
                    _state.PersistAsJson(nameof(UserInfo), new UserInfo
                    {
                        UserId = userIdString,
                        Roles = principal.FindAll(_options.ClaimsIdentity.RoleClaimType).Select(c => c.Value),
                        Permissions = permissions ?? []
                    });
                }
            }
        }

        public void Dispose()
        {
            _subscription.Dispose();
            AuthenticationStateChanged -= OnAuthenticationStateChanged;
        }
    }
}