using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.Administration.Domain;

namespace SmartBuy.Administration.Infrastructure.Configurations
{
    public class DispatcherGroupConfiguration : IEntityTypeConfiguration<DispatcherGroup>
    {
        public void Configure(EntityTypeBuilder<DispatcherGroup> builder)
        {
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Name).HasMaxLength(50);
        }
    }
}
