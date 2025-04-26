using NetCorePal.D3Shop.Domain.DomainEvents.Identity.Admin;
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
        public DateTimeOffset CreatedAt { get; init; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        public virtual ICollection<RolePermission> Permissions { get; init; } = [];

        public Role(string name, string description, IEnumerable<RolePermission> permissions, int status)
        {
            CreatedAt = DateTimeOffset.Now;
            Name = name;
            Description = description;
            Permissions = new List<RolePermission>(permissions);
            Status = status;
        }

        public void UpdateRoleInfo(string name, string description, int status)
        {
            Name = name;
            Description = description;
            Status = status;
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