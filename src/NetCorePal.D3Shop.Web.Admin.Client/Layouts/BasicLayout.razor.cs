using AntDesign.ProLayout;

namespace NetCorePal.D3Shop.Web.Admin.Client.Layouts
{
    public partial class BasicLayout : LayoutComponentBase, IDisposable
    {
        private MenuDataItem[] _menuData = Array.Empty<MenuDataItem>();

        [Inject] private ReuseTabsService TabService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            _menuData = new[] {
                new MenuDataItem
                {
                    Path = "/admin",
                    Name = "welcome",
                    Key = "welcome",
                    Icon = "smile",
                },
                new MenuDataItem
                {
                    Path = "/admin/users",
                    Name = "用户管理",
                    Key = "users",
                    Icon = "crown",
                },
                new MenuDataItem
                {
                    Path = "/admin/roles",
                    Name = "角色管理",
                    Key = "roles",
                    Icon = "crown",
                },
            };
        }

        void Reload()
        {
            TabService.ReloadPage();
        }

        public void Dispose()
        {
            // Method intentionally left empty.
        }

    }
}
