using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate.Dto;
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

        public void AddRoles(List<AddUserRoleDto> addRoleDtoList)
        {
            foreach (var roleDto in addRoleDtoList)
            {
                if (Roles.Any(r => r.RoleId == roleDto.RoleId))
                    continue;

                Roles.Add(new AdminUserRole(Id, roleDto.RoleId, roleDto.RoleName));

                foreach (var permission in roleDto.Permissions)
                {
                    var savedPermission =
                        Permissions.FirstOrDefault(p => p.PermissionCode == permission.PermissionCode);

                    if (savedPermission is null)
                    {
                        var permissionToBeSaved = new AdminUserPermission(
                            Id,
                            permission.PermissionCode,
                            permission.PermissionRemark);
                        permissionToBeSaved.AddSourceRoleId(roleDto.RoleId);
                        Permissions.Add(permissionToBeSaved);
                    }
                    else
                    {
                        savedPermission.AddSourceRoleId(roleDto.RoleId);
                    }
                }
            }
        }

        public void RemoveRoles(List<AdminUserRole> roles)
        {
            foreach (var role in roles)
            {
                Roles.Remove(role);

                var permissions = Permissions.Where(p => p.SourceRoleIds.Contains(role.RoleId)).ToList();
                foreach (var permission in permissions)
                {
                    permission.RemoveSourceRoleId(role.RoleId);
                    if (permission.SourceRoleIds.Count == 0)
                    {
                        Permissions.Remove(permission);
                    }
                }
            }
        }

        public void AddPermissions(List<AddUserPermissionDto> addPermissionDtoList)
        {
            foreach (var addPermissionDto in addPermissionDtoList)
            {
                if (Permissions.Any(p => p.PermissionCode == addPermissionDto.PermissionCode))
                {
                    throw new KnownException("权限重复！");
                }

                Permissions.Add(new AdminUserPermission(Id, addPermissionDto.PermissionCode, addPermissionDto.PermissionRemark));
            }
        }

        public void RemovePermissions(List<string> permissionCodeList)
        {
            foreach (var permissionCode in permissionCodeList)
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
    }
}
