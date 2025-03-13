using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCorePal.D3Shop.Domain.AggregatesModel.DeliverAggregate;

namespace NetCorePal.D3Shop.Infrastructure.EntityConfigurations;

internal class DeliverRecordConfiguration : IEntityTypeConfiguration<DeliverRecord>
{
    public void Configure(EntityTypeBuilder<DeliverRecord> builder)
    {
        builder.ToTable("deliverrecord");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).UseSnowFlakeValueGenerator();
    }
}