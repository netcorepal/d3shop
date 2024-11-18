using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NetCorePal.D3Shop.Admin.Shared.Const;
using NetCorePal.D3Shop.Web.Admin.Client;

namespace NetCorePal.D3Shop.Web.Blazor.Components
{
    // This is a server-side AuthenticationStateProvider that uses PersistentComponentState to flow the
    // authentication state to the client which is then fixed for the lifetime of the WebAssembly application.
    internal sealed class PersistingServerAuthenticationStateProvider : ServerAuthenticationStateProvider, IDisposable
    {
        private readonly PersistentComponentState _state;
        private readonly IdentityOptions _options;

        private readonly PersistingComponentStateSubscription _subscription;

        private Task<AuthenticationState>? _authenticationStateTask;

        public PersistingServerAuthenticationStateProvider(
            PersistentComponentState persistentComponentState,
            IOptions<IdentityOptions> optionsAccessor)
        {
            _state = persistentComponentState;
            _options = optionsAccessor.Value;

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
                var userId = principal.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;

                if (userId != null)
                {
                    _state.PersistAsJson(nameof(UserInfo), new UserInfo
                    {
                        UserId = userId,
                        Roles = principal.FindAll(_options.ClaimsIdentity.RoleClaimType).Select(c => c.Value),
                        Permissions = principal.FindAll(AppClaim.AdminPermission).Select(c => c.Value),
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