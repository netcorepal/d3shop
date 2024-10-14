using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate.Dto;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Infrastructure
{
    public class ApplicationDbSeeder(ApplicationDbContext dbContext)
    {
        public async Task SeedDatabaseAsync()
        {
            //await CheckAndApplyPendingMigrationAsync();
            await SeedRolesAsync();
            await SeedAdminUserAsync();
            await SeedBasicUserAsync();

            await dbContext.SaveChangesAsync();
        }

        private async Task CheckAndApplyPendingMigrationAsync()
        {
            if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
            {
                await dbContext.Database.MigrateAsync();
            }
        }

        private async Task SeedAdminUserAsync()
        {
            if (!await dbContext.AdminUsers.AnyAsync(u => u.Name == AppDefaultCredentials.Name))
            {
                var adminUser = new AdminUser(AppDefaultCredentials.Name, "");
                adminUser.SetPassword(AppDefaultCredentials.Password);

                // Assign role to user
                if (!adminUser.IsInRole(AppDefaultRoles.Basic)
                    || !adminUser.IsInRole(AppDefaultRoles.Admin))
                {
                    var defaultRoles = await dbContext.Roles
                        .Where(r => AppDefaultRoles.DefaultRoles.Contains(r.Name))
                        .ToListAsync();

                    var addRoleDtoList = defaultRoles.Select(r =>
                        new AddUserRoleDto(r.Id, r.Name,
                        r.Permissions.Select(p =>
                                new AddUserPermissionDto(p.PermissionCode, p.PermissionRemark))
                            )).ToList();
                    adminUser.AddRoles(addRoleDtoList);
                }

                await dbContext.AdminUsers.AddAsync(adminUser);
            }
        }

        private async Task SeedBasicUserAsync()
        {
            var basicUser = new AdminUser("Z_jie", "");

            if (!await dbContext.AdminUsers.AnyAsync(u => u.Name == basicUser.Name))
            {
                basicUser.SetPassword(AppDefaultCredentials.Password);

                // Assign role to user
                if (!basicUser.IsInRole(AppDefaultRoles.Basic))
                {
                    var basicRole = await dbContext.Roles.Where(r => r.Name == AppDefaultRoles.Basic).FirstAsync();

                    var addRoleDto = new AddUserRoleDto(
                        basicRole.Id, basicRole.Name,
                        basicRole.Permissions.Select(p =>
                            new AddUserPermissionDto(p.PermissionCode, p.PermissionRemark))
                        );
                    basicUser.AddRoles([addRoleDto]);
                }

                await dbContext.AdminUsers.AddAsync(basicUser);
            }
        }

        private async Task SeedRolesAsync()
        {
            foreach (var roleName in AppDefaultRoles.DefaultRoles)
            {
                if (await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName)
                    is not { } role)
                {
                    role = new Role(roleName, "DefaultRole");

                    await dbContext.Roles.AddAsync(role);
                }

                switch (roleName)
                {
                    case AppDefaultRoles.Admin:
                        AssignPermissionsToRole(role, Permissions.AdminPermissions);
                        break;
                    case AppDefaultRoles.Basic:
                        AssignPermissionsToRole(role, Permissions.BasicPermissions);
                        break;
                }
            }

            await dbContext.SaveChangesAsync();
        }

        private void AssignPermissionsToRole(Role role, IReadOnlyList<Permission> permissions)
        {
            var rolePermissions = role.Permissions;
            foreach (var permission in permissions)
            {
                if (rolePermissions.Any(p => p.PermissionCode == permission.Code)) continue;

                role.Permissions.Add(new RolePermission(role.Id, permission.Code, permission.Remark));
            }
        }
    }
}
