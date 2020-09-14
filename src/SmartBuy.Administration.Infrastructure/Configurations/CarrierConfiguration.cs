using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.Administration.Domain;

namespace SmartBuy.Administration.Infrastructure.Configurations
{
    public class CarrierConfiguration : IEntityTypeConfiguration<Carrier>
    {
        public void Configure(EntityTypeBuilder<Carrier> builder)
        {
            builder.HasKey(c => c.Id);
            builder.OwnsOne(c => c.DeliveryTime)
                     .Property(c => c.Start)
                     .HasColumnName("FromTime");
            builder.OwnsOne(c => c.DeliveryTime)
                   .Property(c => c.End)
                   .HasColumnName("ToTime");
        }
    }
}
