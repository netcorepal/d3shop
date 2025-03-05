using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;

namespace NetCorePal.D3Shop.Infrastructure.EntityConfigurations.Identity;

internal class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).UseSnowFlakeValueGenerator();

        //配置 Department 与 DepartmentUser 的一对多关系
        builder.HasMany(au => au.Users)
            .WithOne()
            .HasForeignKey(aup => aup.DeptId)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder.Navigation(au => au.Users).AutoInclude();
    }

    internal class DepartmentUserConfiguration : IEntityTypeConfiguration<DepartmentUser>
    {
        public void Configure(EntityTypeBuilder<DepartmentUser> builder)
        {
            builder.ToTable("departmentUser");
            builder.HasKey(aur => new { aur.UserId, aur.DeptId });
        }
    }
}