using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Infrastructure.EntityConfigurations.Identity
{
    internal class AdminUserConfiguration : IEntityTypeConfiguration<AdminUser>
    {
        public void Configure(EntityTypeBuilder<AdminUser> builder)
        {
            builder.ToTable("adminUsers");
            builder.HasKey(au => au.Id);
            builder.Property(au => au.Id).ValueGeneratedOnAdd().UseSnowFlakeValueGenerator();
            // 配置 AdminUser 与 AdminUserRole 的一对多关系
            builder.HasMany(au => au.Roles)
                .WithOne()
                .HasForeignKey(aur => aur.AdminUserId)
                .OnDelete(DeleteBehavior.Cascade); // 当删除用户时，级联删除角色关联
            builder.Navigation(au => au.Roles).AutoInclude();

            // 配置 AdminUser 与 AdminUserPermission 的一对多关系
            builder.HasMany(au => au.Permissions)
                .WithOne()
                .HasForeignKey(aup => aup.AdminUserId)
                .OnDelete(DeleteBehavior.Cascade); // 当删除用户时，级联删除权限关联
            builder.Navigation(au => au.Permissions).AutoInclude();
        }
    }

    internal class AdminUserRoleConfiguration : IEntityTypeConfiguration<AdminUserRole>
    {
        public void Configure(EntityTypeBuilder<AdminUserRole> builder)
        {
            builder.ToTable("adminUserRoles");
            builder.HasKey(aur => new { aur.AdminUserId, aur.RoleId });
            builder.HasOne<Role>().WithMany().HasForeignKey(aur => aur.RoleId);
        }
    }

    internal class AdminUserPermissionConfiguration : IEntityTypeConfiguration<AdminUserPermission>
    {
        public void Configure(EntityTypeBuilder<AdminUserPermission> builder)
        {
            builder.ToTable("adminUserPermissions");
            builder.HasKey(aup => new { aup.AdminUserId, aup.PermissionCode });
            builder.Property(p => p.SourceRoleIds).HasConversion(v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<RoleId>>(v, (JsonSerializerOptions?)null) ??
                     new List<RoleId>(),
                new ValueComparer<IList<RoleId>>(
                    (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
        }
    }
}
