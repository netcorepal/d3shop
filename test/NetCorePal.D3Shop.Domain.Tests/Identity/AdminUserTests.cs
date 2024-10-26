using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate.Dto;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Domain.Tests.Identity
{
    public class AdminUserTests
    {
        private readonly AdminUser _testUser = new("test", "1", "", []);

        [Fact]
        public void EditRole_Test()
        {
            const string roleName = "testRole";
            const string rolePermission = "testPermission";
            _testUser.UpdateRoles([
                new AssignAdminUserRoleDto(
                    new RoleId(1),roleName,[new AdminUserPermission(rolePermission,"test")]
                    )
            ]);
            Assert.Contains(_testUser.Roles, r => r.RoleName == roleName);
            Assert.True(_testUser.IsInRole(roleName));
            Assert.Contains(_testUser.Permissions, p => p.PermissionCode == rolePermission);
            _testUser.UpdateRoles([]);
            Assert.Empty(_testUser.Roles);
            Assert.Empty(_testUser.Permissions);
        }
    }
}
