using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Domain.Tests.Identity;

public class RoleTests
{
    [Fact]
    public void EditRolePermission_Test()
    {
        const string rolePermission = "testPermission";
        var role = new Role("testRole", "", [new RolePermission(rolePermission)],0);
        Assert.Contains(role.Permissions, p => p.PermissionCode == rolePermission);
        role.UpdateRolePermissions([]);
        Assert.Empty(role.Permissions);
    }
}