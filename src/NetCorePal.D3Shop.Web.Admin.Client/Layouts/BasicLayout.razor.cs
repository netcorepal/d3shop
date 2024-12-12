using System.Security.Claims;
using AntDesign.ProLayout;
using Microsoft.AspNetCore.Components.Authorization;
using NetCorePal.D3Shop.Admin.Shared.Authorization;
using NetCorePal.D3Shop.Admin.Shared.Permission;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;

namespace NetCorePal.D3Shop.Web.Admin.Client.Layouts;

public partial class BasicLayout : LayoutComponentBase
{
    private PermissionMenuDataItem[] _menuData = [];
    private PermissionMenuDataItem[] _allMenuDate = [];

    [Inject] private ReuseTabsService TabService { get; set; } = default!;
    [Inject] private IPermissionChecker PermissionChecker { get; set; } = default!;

    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _allMenuDate =
        [
            new PermissionMenuDataItem
            {
                Path = "/admin",
                Name = "welcome",
                Key = "welcome",
                Icon = "smile"
            },
            new PermissionMenuDataItem
            {
                Path = "/admin/users",
                Name = "用户管理",
                Key = "users",
                Icon = "crown",
                BoundPermissionCode = PermissionCodes.AdminUserManagement
            },
            new PermissionMenuDataItem
            {
                Path = "/admin/roles",
                Name = "角色管理",
                Key = "roles",
                Icon = "crown",
                BoundPermissionCode = PermissionCodes.RoleManagement
            }
        ];

        var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
        if (user.Identity?.IsAuthenticated is null or false) return;

        // 系统默认用户不校验权限
        var userName = user.Claims.Single(c => c.Type == ClaimTypes.Name).Value;
        if (userName == AppDefaultCredentials.Name)
        {
            _menuData = _allMenuDate;
            return;
        }

        _menuData = await GetAccessibleMenusAsync(user, _allMenuDate); // 过滤出用户有权限的菜单项
    }

    private async Task<PermissionMenuDataItem[]> GetAccessibleMenusAsync(ClaimsPrincipal user,
        PermissionMenuDataItem[] menus)
    {
        var accessibleMenus = new List<PermissionMenuDataItem>();

        foreach (var menu in menus)
        {
            if (menu.BoundPermissionCode is not null &&
                !await PermissionChecker.HasPermissionAsync(user, menu.BoundPermissionCode)) continue;

            var accessibleChildren = await GetAccessibleMenusAsync(user, menu.Children);

            accessibleMenus.Add(new PermissionMenuDataItem
            {
                Path = menu.Path,
                Name = menu.Name,
                Key = menu.Key,
                Icon = menu.Icon,
                BoundPermissionCode = menu.BoundPermissionCode,
                Children = accessibleChildren
            });
        }

        return accessibleMenus.ToArray();
    }


    private void Reload()
    {
        TabService.ReloadPage();
    }
}

internal sealed class PermissionMenuDataItem : MenuDataItem
{
    public string? BoundPermissionCode { get; init; }

    public new PermissionMenuDataItem[] Children { get; init; } = [];
}