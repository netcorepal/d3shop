using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate.Dto;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.Extensions.Domain;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate
{
    public partial record AdminUserId : IInt64StronglyTypedId;

    public class AdminUser : Entity<AdminUserId>, IAggregateRoot
    {
        protected AdminUser() { }

        public string Name { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public string RefreshToken { get; private set; } = string.Empty;
        public DateTime RefreshTokenExpiryDate { get; private set; }
        public DateTime CreatedAt { get; init; }
        public virtual ICollection<AdminUserRole> Roles { get; private set; } = [];
        public virtual ICollection<AdminUserPermission> Permissions { get; private set; } = [];

        public AdminUser(string name, string phone)
        {
            Name = name;
            Phone = phone;
            CreatedAt = DateTime.Now;
        }

        public void UpdateRoleInfo(AdminUserRole role)
        {
            var savedRole = Roles.FirstOrDefault(r => r.RoleId == role.RoleId);
            savedRole?.UpdateRoleInfo(role.RoleName);
        }

        public void AddRoles(IEnumerable<AssignAdminUserRoleDto> rolesToBeAssigned)
        {
            foreach (var roleDto in rolesToBeAssigned)
            {
                if (Roles.Any(r => r.RoleId == roleDto.RoleId))
                    continue;

                Roles.Add(new AdminUserRole(Id, roleDto.RoleId, roleDto.RoleName));

                AddSpecificRolePermissions(roleDto.RoleId, roleDto.Permissions);
            }
        }

        public void RemoveRoles(IEnumerable<RoleId> roleIds)
        {
            var removeList = Roles.Where(role => roleIds.Contains(role.RoleId)).ToList();

            foreach (var role in removeList)
            {
                Roles.Remove(role);
                RemoveSpecificRolePermissions(role.RoleId);
            }
        }

        /// <summary>
        /// 移除特定角色的权限
        /// </summary>
        /// <param name="roleId"></param>
        public void RemoveSpecificRolePermissions(RoleId roleId)
        {
            var permissions = Permissions.Where(p => p.SourceRoleIds.Contains(roleId)).ToArray();
            foreach (var permission in permissions)
            {
                permission.RemoveSourceRoleId(roleId);
                if (permission.SourceRoleIds.Count != 0) continue;
                Permissions.Remove(permission);
            }
        }

        /// <summary>
        /// 添加特定角色的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissions"></param>
        public void AddSpecificRolePermissions(RoleId roleId, IEnumerable<AdminUserPermission> permissions)
        {
            foreach (var permission in permissions)
            {
                var savedPermission =
                    Permissions.FirstOrDefault(p => p.PermissionCode == permission.PermissionCode);

                if (savedPermission is null)
                {
                    permission.AdminUserId = Id;
                    permission.AddSourceRoleId(roleId);
                    Permissions.Add(permission);
                }
                else
                {
                    savedPermission.AddSourceRoleId(roleId);
                }
            }
        }

        public void AddPermissions(IEnumerable<AdminUserPermission> permissionsToBeAssigned)
        {
            foreach (var permission in permissionsToBeAssigned)
            {
                if (Permissions.Any(p => p.PermissionCode == permission.PermissionCode))
                    throw new KnownException("权限重复！");

                permission.AdminUserId = Id;
                Permissions.Add(permission);
            }
        }

        public void RemovePermissions(IEnumerable<string> permissionCodes)
        {
            foreach (var permissionCode in permissionCodes)
            {
                var savedPermission = Permissions.FirstOrDefault(p => p.PermissionCode == permissionCode)
                                      ?? throw new KnownException("用户不存在该权限！");

                if (savedPermission.SourceRoleIds.Count != 0)
                {
                    throw new KnownException("该权限由角色赋予，无法移除！");
                }

                Permissions.Remove(savedPermission);
            }
        }

        public bool IsInRole(string roleName)
        {
            return Roles.Any(r => r.RoleName == roleName);
        }

        public void SetPassword(string password)
        {
            Password = password;
        }

        public void SetRefreshToken(string token)
        {
            RefreshToken = token;
        }

        public void SetRefreshTokenExpiryDate(DateTime date)
        {
            RefreshTokenExpiryDate = date;
        }

        public void SetPhone(string phone)
        {
            Phone = phone;
        }
    }
}
