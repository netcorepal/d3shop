using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate.Dto;
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

        public void AddRolePermissions(List<AddRolePermissionDto> permissions)
        {
            foreach (var permissionDto in permissions)
            {
                if (Permissions.Any(p => p.PermissionCode == permissionDto.PermissionCode))
                    continue;

                Permissions.Add(new RolePermission(Id, permissionDto.PermissionCode, permissionDto.PermissionRemark));
            }
        }

        public void RemoveRolePermissions(List<RolePermission> permissions)
        {
            foreach (var permission in permissions)
            {
                Permissions.Remove(permission);
            }
        }
    }
}
