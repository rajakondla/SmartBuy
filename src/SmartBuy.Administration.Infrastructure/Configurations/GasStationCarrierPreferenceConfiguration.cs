using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.Administration.Domain;

namespace SmartBuy.Administration.Infrastructure.Configurations
{
    public class GasStationCarrierPreferenceConfiguration : IEntityTypeConfiguration<GasStationCarrierPreference>
    {
        public void Configure(EntityTypeBuilder<GasStationCarrierPreference> builder)
        {
            builder.HasOne<Carrier>()
                         .WithMany()
                         .HasForeignKey(c => c.CarrierId);
            builder.HasOne<GasStation>()
                         .WithMany()
                         .HasForeignKey(c => c.GasStationId);
        }
    }
}
