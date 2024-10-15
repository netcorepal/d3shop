using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Domain.Tests.Identity;

public class RoleTests
{
    [Fact]
    public void EditRolePermission_Test()
    {
        const string rolePermission = "testPermission";
        var role = new Role("testRole", "");
        role.AddRolePermissions([
            new RolePermission(role.Id, rolePermission,"test")
        ]);
        Assert.Contains(role.Permissions, p => p.PermissionCode == rolePermission);
        role.RemoveRolePermissions(role.Permissions.Select(p => p.PermissionCode));
        Assert.Empty(role.Permissions);
    }
}