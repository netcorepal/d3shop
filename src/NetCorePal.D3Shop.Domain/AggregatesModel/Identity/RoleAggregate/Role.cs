using NetCorePal.Extensions.Domain;

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
        public ICollection<RolePermission> Permissions { get; init; } = [];

        public Role(string name, string description)
        {
            Name = name;
            Description = description;
            CreatedAt = DateTime.Now;
        }

        public void AddRolePermissions(IEnumerable<RolePermission> permissions)
        {
            foreach (var permission in permissions)
            {
                if (Permissions.Any(p => p.PermissionCode == permission.PermissionCode))
                    continue;

                Permissions.Add(permission);
            }
        }

        public void RemoveRolePermissions(IEnumerable<string> permissionCodes)
        {
            var removeList = Permissions.Where(p =>
                permissionCodes.Contains(p.PermissionCode)).ToList();

            foreach (var permission in removeList)
            {
                Permissions.Remove(permission);
            }
        }
    }
}
