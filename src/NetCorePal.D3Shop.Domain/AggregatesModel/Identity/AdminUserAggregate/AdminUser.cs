using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Domain.DomainEvents.Identity.Admin;
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

        public string RealName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; init; }
        public virtual ICollection<AdminUserRole> Roles { get; } = [];

        public virtual ICollection<UserDept> UserDepts { get; } = [];



        public virtual ICollection<AdminUserPermission> Permissions { get; } = [];
        public bool IsDeleted { get; private set; }
        public DateTimeOffset? DeletedAt { get; private set; }

        public AdminUser(string name, string phone, string password,
            IEnumerable<AdminUserRole> roles, IEnumerable<AdminUserPermission> permissions)
        {
            CreatedAt = DateTimeOffset.Now;
            Name = name;
            Phone = phone;
            Password = password;
            foreach (var adminUserRole in roles)
            {
                Roles.Add(adminUserRole);
            }

            foreach (var adminUserPermission in permissions)
            {
                Permissions.Add(adminUserPermission);
            }
        }

        public void UpdateRoleInfo(RoleId roleId, string roleName)
        {
            var savedRole = Roles.FirstOrDefault(r => r.RoleId == roleId);
            savedRole?.UpdateRoleInfo(roleName);
        }

        public void SetUserDepts(DeptId deptId, string deptName)
        {
            var savedDept = UserDepts.FirstOrDefault(r => r.DeptId == deptId);
            savedDept?.UpdateDeptInfo(deptName);
        }

        /// <summary>
        /// 添加用户到部门
        /// </summary>
        /// <param name="deptId">部门ID</param>
        /// <param name="deptName">部门名称</param>
        public void AddUserDept(DeptId deptId, string deptName)
        {
            // 检查用户是否已在该部门
            if (UserDepts.Any(d => d.DeptId == deptId))
            {
                return;
            }

            var userDept = new UserDept(deptId, deptName);
            UserDepts.Add(userDept);

            // 添加领域事件，通知部门用户数量增加
            AddDomainEvent(new UserDeptChangedDomainEvent(userDept));
        }

        /// <summary>
        /// 从部门中移除用户
        /// </summary>
        /// <param name="deptId">部门ID</param>
        public void RemoveUserDept(DeptId deptId)
        {
            var userDept = UserDepts.FirstOrDefault(d => d.DeptId == deptId);
            if (userDept == null)
            {
                return;
            }

            UserDepts.Remove(userDept);
            // 添加领域事件，通知部门用户数量减少
            AddDomainEvent(new UserDeptChangedDomainEvent(userDept));
        }

        public void UpdateRoles(IEnumerable<AdminUserRole> rolesToBeAssigned,
            IEnumerable<AdminUserPermission> permissions)
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
                Roles.Add(targetRole);
            }

            AddPermissions(permissions);
        }

        public void UpdateRolePermissions(RoleId roleId, IEnumerable<AdminUserPermission> newPermissions)
        {
            RemoveRolePermissions(roleId);
            AddPermissions(newPermissions);
        }

        private void AddPermissions(IEnumerable<AdminUserPermission> permissions)
        {
            foreach (var permission in permissions)
            {
                var existedPermission = Permissions.SingleOrDefault(p => p.PermissionCode == permission.PermissionCode);
                if (existedPermission is not null)
                {
                    foreach (var permissionSourceRoleId in permission.SourceRoleIds)
                        existedPermission.AddSourceRoleId(permissionSourceRoleId);
                }
                else
                {
                    Permissions.Add(permission);
                }
            }
        }

        private void RemoveRolePermissions(RoleId roleId)
        {
            foreach (var permission in Permissions.Where(
                             p => p.SourceRoleIds.Remove(roleId) &&
                                  p.SourceRoleIds.Count == 0)
                         .ToArray())
            {
                Permissions.Remove(permission);
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
            DeletedAt = DateTimeOffset.Now;
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