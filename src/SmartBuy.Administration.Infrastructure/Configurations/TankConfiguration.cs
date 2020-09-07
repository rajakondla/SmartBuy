using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.Administration.Domain;

namespace SmartBuy.Administration.Infrastructure.Configurations
{
    public class TankConfiguration : IEntityTypeConfiguration<Tank>
    {
        public void Configure(EntityTypeBuilder<Tank> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).HasMaxLength(50);
            builder.HasOne<Product>()
                        .WithMany()
                        .HasForeignKey(t => t.ProductId);
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
