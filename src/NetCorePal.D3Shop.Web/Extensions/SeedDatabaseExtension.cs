using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Web.Helper;

namespace NetCorePal.D3Shop.Web.Extensions
{
    public static class SeedDatabaseExtension
    {
        internal static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // 初始化管理员用户
            if (!dbContext.AdminUsers.Any(u => u.Name == AppDefaultCredentials.Name))
            {
                var adminUser = new AdminUser(AppDefaultCredentials.Name, "",
                    PasswordHasher.HashPassword(AppDefaultCredentials.Password), [], []);
                dbContext.AdminUsers.Add(adminUser);
                dbContext.SaveChanges();
            }

            // 初始化菜单数据
            if (!dbContext.Menus.Any())
            {
                // 创建仪表盘菜单
                var dashboardMenu = new Menu(
                    "概览",
                    "/dashboard",
                    MenuType.Catalog,
                    new MenuId(0),
                    "DASHBOARD",
                    "Layout",
                    "",
                    1,
                    "ion:grid-outline",
                    1,
                    new MenuMeta
                    {
                        Title = "概览",
                        Icon = "ion:grid-outline",
                        Order = 1,
                        AffixTab = true,
                        HideChildrenInMenu = false
                    }
                );
                dbContext.Menus.Add(dashboardMenu);
                dbContext.SaveChanges();

                // 创建分析页菜单
                var analysisMenu = new Menu(
                    "分析页",
                    "/dashboard/analysis",
                    MenuType.Menu,
                    dashboardMenu.Id,
                    "DASHBOARD_ANALYSIS",
                    "dashboard/analysis/index",
                    "",
                    1,
                    "ion:bar-chart-outline",
                    1,
                    new MenuMeta
                    {
                        Title = "分析页",
                        Icon = "ion:bar-chart-outline",
                        Order = 1,
                        KeepAlive = true
                    }
                );
                dbContext.Menus.Add(analysisMenu);
                dbContext.SaveChanges();

                // 创建系统管理菜单
                var systemMenu = new Menu(
                    "系统管理",
                    "/system",
                    MenuType.Catalog,
                    new MenuId(0),
                    "SYSTEM_MANAGE",
                    "Layout",
                    "",
                    2,
                    "ion:settings-outline",
                    1,
                    new MenuMeta
                    {
                        Title = "系统管理",
                        Icon = "ion:settings-outline",
                        Order = 2,
                        HideChildrenInMenu = false
                    }
                );
                dbContext.Menus.Add(systemMenu);
                dbContext.SaveChanges();

                //// 创建账号管理菜单
                //var accountMenu = new Menu(
                //    "账号管理",
                //    "/system/account",
                //    MenuType.Menu,
                //    systemMenu.Id,
                //    "ACCOUNT_MANAGE",
                //    "system/account/index",
                //    "",
                //    1,
                //    "ion:person-outline",
                //    1,
                //    new MenuMeta
                //    {
                //        Title = "账号管理",
                //        Icon = "ion:person-outline",
                //        Order = 1,
                //        KeepAlive = true
                //    }
                //);
                //dbContext.Menus.Add(accountMenu);
                //dbContext.SaveChanges();

                //// 创建账号管理操作按钮
                //var accountButtons = new List<Menu>
                //{
                //    new Menu(
                //        "新增账号",
                //        "/system/account/add",
                //        MenuType.Button,
                //        accountMenu.Id,
                //        "ACCOUNT_ADD",
                //        "",
                //        "",
                //        1,
                //        "ion:add-circle-outline",
                //        1,
                //        new MenuMeta
                //        {
                //            Title = "新增账号",
                //            Icon = "ion:add-circle-outline",
                //            Order = 1
                //        }
                //    ),
                //    new Menu(
                //        "编辑账号",
                //        "/system/account/edit",
                //        MenuType.Button,
                //        accountMenu.Id,
                //        "ACCOUNT_EDIT",
                //        "",
                //        "",
                //        2,
                //        "ion:create-outline",
                //        1,
                //        new MenuMeta
                //        {
                //            Title = "编辑账号",
                //            Icon = "ion:create-outline",
                //            Order = 2
                //        }
                //    ),
                //    new Menu(
                //        "删除账号",
                //        "/system/account/delete",
                //        MenuType.Button,
                //        accountMenu.Id,
                //        "ACCOUNT_DELETE",
                //        "",
                //        "",
                //        3,
                //        "ion:trash-outline",
                //        1,
                //        new MenuMeta
                //        {
                //            Title = "删除账号",
                //            Icon = "ion:trash-outline",
                //            Order = 3
                //        }
                //    ),
                //    new Menu(
                //        "重置密码",
                //        "/system/account/reset-password",
                //        MenuType.Button,
                //        accountMenu.Id,
                //        "ACCOUNT_RESET_PASSWORD",
                //        "",
                //        "",
                //        4,
                //        "ion:key-outline",
                //        1,
                //        new MenuMeta
                //        {
                //            Title = "重置密码",
                //            Icon = "ion:key-outline",
                //            Order = 4
                //        }
                //    )
                //};
                //dbContext.Menus.AddRange(accountButtons);

                // 创建角色管理菜单
                var roleMenu = new Menu(
                    "角色管理",
                    "/system/role",
                    MenuType.Menu,
                    systemMenu.Id,
                    "ROLE_MANAGE",
                    "system/role/index",
                    "",
                    2,
                    "ion:people-outline",
                    1,
                    new MenuMeta
                    {
                        Title = "角色管理",
                        Icon = "ion:people-outline",
                        Order = 2,
                        KeepAlive = true
                    }
                );
                dbContext.Menus.Add(roleMenu);
                dbContext.SaveChanges();

                // 创建角色管理操作按钮
                var roleButtons = new List<Menu>
                {
                    new Menu(
                        "新增角色",
                        "/system/role/add",
                        MenuType.Button,
                        roleMenu.Id,
                        "ROLE_ADD",
                        "",
                        "",
                        1,
                        "ion:add-circle-outline",
                        1,
                        new MenuMeta
                        {
                            Title = "新增角色",
                            Icon = "ion:add-circle-outline",
                            Order = 1
                        }
                    ),
                    new Menu(
                        "编辑角色",
                        "/system/role/edit",
                        MenuType.Button,
                        roleMenu.Id,
                        "ROLE_EDIT",
                        "",
                        "",
                        2,
                        "ion:create-outline",
                        1,
                        new MenuMeta
                        {
                            Title = "编辑角色",
                            Icon = "ion:create-outline",
                            Order = 2
                        }
                    ),
                    new Menu(
                        "删除角色",
                        "/system/role/delete",
                        MenuType.Button,
                        roleMenu.Id,
                        "ROLE_DELETE",
                        "",
                        "",
                        3,
                        "ion:trash-outline",
                        1,
                        new MenuMeta
                        {
                            Title = "删除角色",
                            Icon = "ion:trash-outline",
                            Order = 3
                        }
                    ),
                    new Menu(
                        "分配权限",
                        "/system/role/assign-permission",
                        MenuType.Button,
                        roleMenu.Id,
                        "ROLE_ASSIGN_PERMISSION",
                        "",
                        "",
                        4,
                        "ion:key-outline",
                        1,
                        new MenuMeta
                        {
                            Title = "分配权限",
                            Icon = "ion:key-outline",
                            Order = 4
                        }
                    )
                };
                dbContext.Menus.AddRange(roleButtons);

                // 创建菜单管理菜单
                var menuManageMenu = new Menu(
                    "菜单管理",
                    "/system/menu",
                    MenuType.Menu,
                    systemMenu.Id,
                    "MENU_MANAGE",
                    "system/menu/index",
                    "",
                    3,
                    "ion:menu-outline",
                    1,
                    new MenuMeta
                    {
                        Title = "菜单管理",
                        Icon = "ion:menu-outline",
                        Order = 3,
                        KeepAlive = true
                    }
                );
                dbContext.Menus.Add(menuManageMenu);
                dbContext.SaveChanges();

                // 创建菜单管理操作按钮
                var menuManageButtons = new List<Menu>
                {
                    new Menu(
                        "新增菜单",
                        "/system/menu/add",
                        MenuType.Button,
                        menuManageMenu.Id,
                        "MENU_ADD",
                        "",
                        "",
                        1,
                        "ion:add-circle-outline",
                        1,
                        new MenuMeta
                        {
                            Title = "新增菜单",
                            Icon = "ion:add-circle-outline",
                            Order = 1
                        }
                    ),
                    new Menu(
                        "编辑菜单",
                        "/system/menu/edit",
                        MenuType.Button,
                        menuManageMenu.Id,
                        "MENU_EDIT",
                        "",
                        "",
                        2,
                        "ion:create-outline",
                        1,
                        new MenuMeta
                        {
                            Title = "编辑菜单",
                            Icon = "ion:create-outline",
                            Order = 2
                        }
                    ),
                    new Menu(
                        "删除菜单",
                        "/system/menu/delete",
                        MenuType.Button,
                        menuManageMenu.Id,
                        "MENU_DELETE",
                        "",
                        "",
                        3,
                        "ion:trash-outline",
                        1,
                        new MenuMeta
                        {
                            Title = "删除菜单",
                            Icon = "ion:trash-outline",
                            Order = 3
                        }
                    )
                };
                dbContext.Menus.AddRange(menuManageButtons);

                // 创建部门管理菜单
                var deptMenu = new Menu(
                    "部门管理",
                    "/system/dept",
                    MenuType.Menu,
                    systemMenu.Id,
                    "DEPT_MANAGE",
                    "system/dept/index",
                    "",
                    4,
                    "ion:git-network-outline",
                    1,
                    new MenuMeta
                    {
                        Title = "部门管理",
                        Icon = "ion:git-network-outline",
                        Order = 4,
                        KeepAlive = true
                    }
                );
                dbContext.Menus.Add(deptMenu);
                dbContext.SaveChanges();

                // 创建部门管理操作按钮
                var deptButtons = new List<Menu>
                {
                    new Menu(
                        "新增部门",
                        "/system/dept/add",
                        MenuType.Button,
                        deptMenu.Id,
                        "DEPT_ADD",
                        "",
                        "",
                        1,
                        "ion:add-circle-outline",
                        1,
                        new MenuMeta
                        {
                            Title = "新增部门",
                            Icon = "ion:add-circle-outline",
                            Order = 1
                        }
                    ),
                    new Menu(
                        "编辑部门",
                        "/system/dept/edit",
                        MenuType.Button,
                        deptMenu.Id,
                        "DEPT_EDIT",
                        "",
                        "",
                        2,
                        "ion:create-outline",
                        1,
                        new MenuMeta
                        {
                            Title = "编辑部门",
                            Icon = "ion:create-outline",
                            Order = 2
                        }
                    ),
                    new Menu(
                        "删除部门",
                        "/system/dept/delete",
                        MenuType.Button,
                        deptMenu.Id,
                        "DEPT_DELETE",
                        "",
                        "",
                        3,
                        "ion:trash-outline",
                        1,
                        new MenuMeta
                        {
                            Title = "删除部门",
                            Icon = "ion:trash-outline",
                            Order = 3
                        }
                    )
                };
                dbContext.Menus.AddRange(deptButtons);
                dbContext.SaveChanges();
            }

            return app;
        }
    }
}