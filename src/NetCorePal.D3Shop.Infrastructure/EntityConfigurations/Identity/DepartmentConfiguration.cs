using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;

namespace NetCorePal.D3Shop.Infrastructure.EntityConfigurations.Identity
{
 
    internal class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("departments");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedOnAdd().UseSnowFlakeValueGenerator();
        }
    }
}
