using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.Administration.Domain;

namespace SmartBuy.Administration.Infrastructure.Configurations
{
    public class CarrierFreightConfiguration : IEntityTypeConfiguration<CarrierFreight>
    {
        public void Configure(EntityTypeBuilder<CarrierFreight> builder)
        {
            builder.HasOne<Carrier>()
                         .WithMany()
                         .HasForeignKey(c => c.CarrierId);
            builder.HasOne<Terminal>()
                        .WithMany()
                        .HasForeignKey(c => c.TerminalId);
            builder.HasOne<GasStation>()
                        .WithMany()
                        .HasForeignKey(c => c.GasStationId);
            builder.OwnsOne(c => c.Cost)
                   .Property(c => c.Value)
                   .HasColumnName("FreightCost");
            builder.OwnsOne(c => c.Cost)
                   .Property(c => c.Unit)
                   .HasColumnName("MoneyUnit");
        }
    }
}
