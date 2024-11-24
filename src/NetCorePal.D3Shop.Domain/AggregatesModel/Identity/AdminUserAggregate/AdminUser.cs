using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate.Dto;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.Extensions.Domain;
using NetCorePal.Extensions.Primitives;

// ReSharper disable VirtualMemberCallInConstructor

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate
{
    public partial record AdminUserId : IInt64StronglyTypedId;

    public class AdminUser : Entity<AdminUserId>, IAggregateRoot
    {
        protected AdminUser()
        {
        }

        public string Name { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public virtual ICollection<AdminUserRole> Roles { get; } = [];
        public virtual ICollection<AdminUserPermission> Permissions { get; } = [];
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public AdminUser(string name, string phone, string password,
            IEnumerable<AssignAdminUserRoleDto> rolesToBeAssigned)
        {
            CreatedAt = DateTime.Now;
            Name = name;
            Phone = phone;
            Password = password;
            foreach (var roleDto in rolesToBeAssigned)
            {
                Roles.Add(new AdminUserRole(roleDto.RoleId, roleDto.RoleName));
                foreach (var rolePermission in roleDto.Permissions)
                {
                    rolePermission.SourceRoleIds.Add(roleDto.RoleId);
                    Permissions.Add(rolePermission);
                }
            }
        }

        public void UpdateRoleInfo(RoleId roleId, string roleName)
        {
            var savedRole = Roles.FirstOrDefault(r => r.RoleId == roleId);
            savedRole?.UpdateRoleInfo(roleName);
        }

        public void UpdateRoles(IEnumerable<AssignAdminUserRoleDto> rolesToBeAssigned)
        {
            var currentRoleMap = Roles.ToDictionary(r => r.RoleId);
            var targetRoleMap = rolesToBeAssigned.ToDictionary(r => r.RoleId);

            var roleIdsToRemove = currentRoleMap.Keys.Except(targetRoleMap.Keys);
            foreach (var roleId in roleIdsToRemove)
            {
                Roles.Remove(currentRoleMap[roleId]);
                RemoveRolePermissions(roleId);
            }

            var roleIdsToAdd = targetRoleMap.Keys.Except(currentRoleMap.Keys);
            foreach (var roleId in roleIdsToAdd)
            {
                var targetRole = targetRoleMap[roleId];
                Roles.Add(new AdminUserRole(roleId, targetRole.RoleName));
                AddRolePermissions(roleId, targetRole.Permissions);
            }
        }

        public void UpdateRolePermissions(RoleId roleId, IEnumerable<AdminUserPermission> newPermissions)
        {
            RemoveRolePermissions(roleId);
            AddRolePermissions(roleId, newPermissions);
        }

        private void RemoveRolePermissions(RoleId roleId)
        {
            foreach (var permission in Permissions.Where(p => p.SourceRoleIds.Remove(roleId)).ToArray())
            {
                if (permission.SourceRoleIds.Count == 0)
                    Permissions.Remove(permission);
            }
        }

        private void AddRolePermissions(RoleId roleId, IEnumerable<AdminUserPermission> permissions)
        {
            foreach (var permission in permissions)
            {
                var existingPermission = Permissions.FirstOrDefault(p => p.PermissionCode == permission.PermissionCode);

                if (existingPermission is null)
                {
                    permission.AddSourceRoleId(roleId);
                    Permissions.Add(permission);
                }
                else
                {
                    existingPermission.AddSourceRoleId(roleId);
                }
            }
        }

        public void SetSpecificPermissions(IEnumerable<AdminUserPermission> permissionsToBeAssigned)
        {
            var currentSpecificPermissionMap =
                Permissions.Where(p => p.SourceRoleIds.Count == 0).ToDictionary(p => p.PermissionCode);
            var newSpecificPermissionMap = permissionsToBeAssigned.ToDictionary(p => p.PermissionCode);

            var permissionCodesToRemove = currentSpecificPermissionMap.Keys.Except(newSpecificPermissionMap.Keys);
            foreach (var permissionCode in permissionCodesToRemove)
            {
                var permission = currentSpecificPermissionMap[permissionCode];
                Permissions.Remove(permission);
            }

            var permissionCodesToAdd = newSpecificPermissionMap.Keys.Except(currentSpecificPermissionMap.Keys);
            foreach (var permissionCode in permissionCodesToAdd)
            {
                if (Permissions.Any(p => p.PermissionCode == permissionCode))
                    throw new KnownException("权限重复！");
                Permissions.Add(newSpecificPermissionMap[permissionCode]);
            }
        }

        public void Delete()
        {
            if (IsDeleted) throw new KnownException("用户已经被删除！");
            IsDeleted = true;
            DeletedAt = DateTime.Now;
        }

        public bool IsInRole(string roleName)
        {
            return Roles.Any(r => r.RoleName == roleName);
        }

        public void SetPassword(string password)
        {
            Password = password;
        }

        public void SetPhone(string phone)
        {
            Phone = phone;
        }
    }
}