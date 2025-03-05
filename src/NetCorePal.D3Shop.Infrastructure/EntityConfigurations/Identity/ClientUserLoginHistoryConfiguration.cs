using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserLoginHistoryAggregate;

namespace NetCorePal.D3Shop.Infrastructure.EntityConfigurations.Identity;

internal class ClientUserLoginHistoryConfiguration : IEntityTypeConfiguration<ClientUserLoginHistory>
{
    public void Configure(EntityTypeBuilder<ClientUserLoginHistory> builder)
    {
        builder.ToTable("clientUserLoginHistory");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).UseSnowFlakeValueGenerator();
    }
}