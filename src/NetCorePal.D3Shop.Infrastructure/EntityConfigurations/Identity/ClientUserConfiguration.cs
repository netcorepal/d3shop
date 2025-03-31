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
        builder.Property(cu => cu.Id).UseSnowFlakeValueGenerator();
        // 配置 ClientUser 与 DeliveryAddress 的一对多关系
        builder.HasMany(cu => cu.DeliveryAddresses)
            .WithOne()
            .HasForeignKey(uda => uda.UserId)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder.Navigation(cu => cu.DeliveryAddresses).AutoInclude();

        // 配置 ClientUser 与 ThirdPartyLogin 的一对多关系
        builder.HasMany(cu => cu.ThirdPartyLogins)
            .WithOne()
            .HasForeignKey(tpl => tpl.UserId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasMany(cu => cu.RefreshTokens)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}

internal class UserDeliveryAddressConfiguration : IEntityTypeConfiguration<UserDeliveryAddress>
{
    public void Configure(EntityTypeBuilder<UserDeliveryAddress> builder)
    {
        builder.ToTable("userDeliveryAddresses");
        builder.HasKey(uda => uda.Id);
        builder.Property(uda => uda.Id).UseSnowFlakeValueGenerator();
    }
}

internal class UserThirdPartyLoginConfiguration : IEntityTypeConfiguration<UserThirdPartyLogin>
{
    public void Configure(EntityTypeBuilder<UserThirdPartyLogin> builder)
    {
        builder.ToTable("userThirdPartyLogins");
        builder.HasKey(tpl => tpl.Id);
        builder.Property(tpl => tpl.Id).UseSnowFlakeValueGenerator();
        builder.Property(x => x.Provider)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<ThirdPartyProvider>(v));
    }
}

internal class ClientUserRefreshTokenConfiguration : IEntityTypeConfiguration<ClientUserRefreshToken>
{
    public void Configure(EntityTypeBuilder<ClientUserRefreshToken> builder)
    {
        builder.ToTable("clientUserRefreshTokens");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseSnowFlakeValueGenerator();
    }
}