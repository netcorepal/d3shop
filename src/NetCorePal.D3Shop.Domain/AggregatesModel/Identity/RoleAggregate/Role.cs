using NetCorePal.D3Shop.Domain.DomainEvents.Identity;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate
{
    public partial record RoleId : IInt64StronglyTypedId;

    public class Role : Entity<RoleId>, IAggregateRoot
    {
        protected Role() { }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public virtual ICollection<RolePermission> Permissions { get; init; } = [];

        public Role(string name, string description)
        {
            Name = name;
            Description = description;
            CreatedAt = DateTime.Now;
        }

        public void UpdateRoleInfo(string name, string description)
        {
            Name = name;
            Description = description;
            AddDomainEvent(new RoleInfoChangedDomainEvent(this));
        }

        public void AddRolePermissions(IEnumerable<RolePermission> permissions)
        {
            foreach (var permission in permissions)
            {
                if (Permissions.Any(p => p.PermissionCode == permission.PermissionCode))
                    continue;

                permission.RoleId = Id;
                Permissions.Add(permission);
            }
            AddDomainEvent(new RolePermissionChangedDomainEvent(this));
        }

        public void RemoveRolePermissions(IEnumerable<string> permissionCodes)
        {
            var removeList = Permissions.Where(p =>
                permissionCodes.Contains(p.PermissionCode)).ToList();

            foreach (var permission in removeList)
            {
                Permissions.Remove(permission);
            }
            AddDomainEvent(new RolePermissionChangedDomainEvent(this));
        }
    }
}
