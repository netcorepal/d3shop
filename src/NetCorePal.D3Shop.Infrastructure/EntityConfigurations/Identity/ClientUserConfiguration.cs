using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

namespace NetCorePal.D3Shop.Infrastructure.EntityConfigurations.Identity;

internal class ClientUserConfiguration : IEntityTypeConfiguration<ClientUser>
{
    public void Configure(EntityTypeBuilder<ClientUser> builder)
    {
        builder.ToTable("clientUsers");
        builder.HasKey(cu => cu.Id);
        builder.Property(cu => cu.Id).ValueGeneratedOnAdd().UseSnowFlakeValueGenerator();
        // 配置 ClientUser 与 DeliveryAddress 的一对多关系
        builder.HasMany(cu => cu.DeliveryAddresses)
            .WithOne()
            .HasForeignKey(uda => uda.UserId)
            .OnDelete(DeleteBehavior.ClientCascade);
        // 配置 ClientUser 与 ThirdPartyLogin 的一对多关系
        builder.HasMany(cu => cu.ThirdPartyLogins)
            .WithOne()
            .HasForeignKey(tpl => tpl.UserId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}

internal class UserDeliveryAddressConfiguration : IEntityTypeConfiguration<UserDeliveryAddress>
{
    public void Configure(EntityTypeBuilder<UserDeliveryAddress> builder)
    {
        builder.ToTable("userDeliveryAddresses");
        builder.HasKey(uda => uda.Id);
        builder.Property(uda => uda.Id).ValueGeneratedOnAdd().UseSnowFlakeValueGenerator();
    }
}

internal class UserThirdPartyLoginConfiguration : IEntityTypeConfiguration<UserThirdPartyLogin>
{
    public void Configure(EntityTypeBuilder<UserThirdPartyLogin> builder)
    {
        builder.ToTable("userThirdPartyLogins");
        builder.HasKey(tpl => tpl.Id);
        builder.Property(tpl => tpl.Id).ValueGeneratedOnAdd().UseSnowFlakeValueGenerator();
    }
}