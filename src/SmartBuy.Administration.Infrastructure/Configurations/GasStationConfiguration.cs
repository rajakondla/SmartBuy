using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.Administration.Domain;
using System;


namespace SmartBuy.Administration.Infrastructure.Configurations
{
    public class GasStationConfiguration : IEntityTypeConfiguration<GasStation>
    {
        public void Configure(EntityTypeBuilder<GasStation> builder)
        {
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Name).HasMaxLength(50);
            builder.Property(g => g.Address).HasMaxLength(200);
            builder.HasOne<DispatcherGroup>()
                        .WithMany()
                        .HasForeignKey(t => t.DispatcherGroupId);
            builder.OwnsOne(g => g.DeliveryTime)
               .Property(g => g.Start)
               .HasColumnName("FromTime");
            builder.OwnsOne(g => g.DeliveryTime)
              .Property(g => g.End)
              .HasColumnName("ToTime");
        }
    }
}
