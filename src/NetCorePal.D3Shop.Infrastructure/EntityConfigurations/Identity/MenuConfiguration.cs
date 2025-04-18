using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;

namespace NetCorePal.D3Shop.Infrastructure.EntityConfigurations.Identity;

internal class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("menus");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).UseSnowFlakeValueGenerator();

        //// 配置 Menu 与子菜单的一对多关系
        //builder.HasMany(m => m.Children)
        //    .WithOne(m => m.Parent)
        //    .HasForeignKey(m => m.ParentId)
        //    .OnDelete(DeleteBehavior.Restrict);

        // 配置 ParentId 列名
        builder.Property(m => m.ParentId)
            .HasColumnName("ParentId");

        // 配置 MenuMeta 的 JSON 序列化
        builder.Property(m => m.Meta).HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<MenuMeta>(v, (JsonSerializerOptions?)null),
            new ValueComparer<MenuMeta>(
                (c1, c2) => c1 != null && c2 != null &&
                    c1.Title == c2.Title &&
                    c1.Icon == c2.Icon &&
                    c1.Order == c2.Order &&
                    c1.HideInMenu == c2.HideInMenu &&
                    c1.HideInTab == c2.HideInTab &&
                    c1.KeepAlive == c2.KeepAlive &&
                    c1.AffixTab == c2.AffixTab &&
                    c1.HideInBreadcrumb == c2.HideInBreadcrumb &&
                    c1.HideChildrenInMenu == c2.HideChildrenInMenu &&
                    c1.OpenInNewWindow == c2.OpenInNewWindow &&
                    c1.NoBasicLayout == c2.NoBasicLayout &&
                    c1.MaxNumOfOpenTab == c2.MaxNumOfOpenTab,
                c => c.GetHashCode(),
                c => c));

        // 配置必填字段
        builder.Property(m => m.Name).IsRequired();
        builder.Property(m => m.Path).IsRequired();
        builder.Property(m => m.Type).IsRequired();

        // 配置索引
        builder.HasIndex(m => m.ParentId);
        builder.HasIndex(m => m.Path).IsUnique();
        builder.HasIndex(m => m.Name).IsUnique();
    }
}