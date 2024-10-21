using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Infrastructure.EntityConfigurations.Identity
{
    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedOnAdd().UseSnowFlakeValueGenerator();
            builder.HasMany(r => r.Permissions).WithOne().HasForeignKey(rp => rp.RoleId);
            builder.Navigation(e => e.Permissions).AutoInclude();
        }
    }

    internal class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("rolePermissions");
            builder.HasKey(rp => new { rp.RoleId, rp.PermissionCode });
        }
    }
}
