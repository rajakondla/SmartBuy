using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.OrderManagement.Infrastructure.Configurations
{
    public class TankConfiguration : IEntityTypeConfiguration<Tank>
    {
        public void Configure(EntityTypeBuilder<Tank> builder)
        {
            builder.OwnsOne(t => t.Measurement)
                .Property(t => t.NetQuantity)
                .HasColumnName("NetQuantity");
            builder.OwnsOne(t => t.Measurement)
                .Property(t => t.Unit)
                .HasColumnName("Unit");
            builder.OwnsOne(t => t.Measurement)
                .Property(t => t.Quantity)
                .HasColumnName("Quantity");
            builder.OwnsOne(t => t.Measurement)
                .Property(t => t.Top)
                .HasColumnName("Top");
            builder.OwnsOne(t => t.Measurement)
                .Property(t => t.Bottom)
                .HasColumnName("Bottom");
        }
    }
}
