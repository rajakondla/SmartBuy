using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.Administration.Domain;

namespace SmartBuy.Administration.Infrastructure.Configurations
{
    public class GasStationTerminalPreferenceConfiguration : IEntityTypeConfiguration<GasStationTerminalPreference>
    {
        public void Configure(EntityTypeBuilder<GasStationTerminalPreference> builder)
        {
            builder.HasOne<Terminal>()
                         .WithMany()
                         .HasForeignKey(c => c.TerminalId);
            builder.HasOne<GasStation>()
                         .WithMany()
                         .HasForeignKey(c => c.GasStationId);
        }
    }
}
