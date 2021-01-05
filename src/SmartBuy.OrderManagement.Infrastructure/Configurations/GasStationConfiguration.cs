using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.OrderManagement.Domain;

namespace SmartBuy.OrderManagement.Infrastructure.Configurations
{
    public class GasStationConfiguration : IEntityTypeConfiguration<GasStation>
    {
        public void Configure(EntityTypeBuilder<GasStation> builder)
        {
            builder.OwnsOne(x => x.DeliveryTime)
                .Property(x => x.Start)
                .HasColumnName("FromTime");
            builder.OwnsOne(x => x.DeliveryTime)
                .Property(x => x.End)
                .HasColumnName("ToTime");
        }
    }
}
