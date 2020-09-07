
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.Administration.Domain;

namespace SmartBuy.Administration.Infrastructure.Configurations
{
    public class GasStationScheduleByTimeConfiguration : IEntityTypeConfiguration<GasStationScheduleByTime>
    {
        public void Configure(EntityTypeBuilder<GasStationScheduleByTime> builder)
        {
            builder.HasOne<GasStation>()
                            .WithMany()
                            .HasForeignKey(t => t.GasStationId);

            builder.HasKey(x => new { x.GasStationId, x.TimeInteral });
        }
    }
}
