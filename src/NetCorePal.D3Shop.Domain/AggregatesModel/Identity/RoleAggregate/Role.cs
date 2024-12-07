using NetCorePal.D3Shop.Domain.DomainEvents.Identity;
using NetCorePal.Extensions.Domain;
// ReSharper disable VirtualMemberCallInConstructor

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate
{
    public partial record RoleId : IInt64StronglyTypedId;

    public class Role : Entity<RoleId>, IAggregateRoot
    {
        protected Role()
        {
        }

        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public virtual ICollection<RolePermission> Permissions { get; init; } = [];

        public Role(string name, string description, IEnumerable<RolePermission> permissions)
        {
            CreatedAt = DateTime.Now;
            Name = name;
            Description = description;
            Permissions = new List<RolePermission>(permissions);
        }

        public void UpdateRoleInfo(string name, string description)
        {
            Name = name;
            Description = description;
            AddDomainEvent(new RoleInfoChangedDomainEvent(this));
        }

        public void UpdateRolePermissions(IEnumerable<RolePermission> newPermissions)
        {
            var currentPermissionMap = Permissions.ToDictionary(p => p.PermissionCode);
            var newPermissionMap = newPermissions.ToDictionary(p => p.PermissionCode);

            var permissionsToRemove = currentPermissionMap.Keys.Except(newPermissionMap.Keys).ToList();
            foreach (var permissionCode in permissionsToRemove)
            {
                Permissions.Remove(currentPermissionMap[permissionCode]);
            }

            var permissionsToAdd = newPermissionMap.Keys.Except(currentPermissionMap.Keys).ToList();
            foreach (var permissionCode in permissionsToAdd)
            {
                Permissions.Add(newPermissionMap[permissionCode]);
            }

            AddDomainEvent(new RolePermissionChangedDomainEvent(this));
        }
    }
}