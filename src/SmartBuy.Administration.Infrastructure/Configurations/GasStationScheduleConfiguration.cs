
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.Administration.Domain;

namespace SmartBuy.Administration.Infrastructure.Configurations
{
    public class GasStationScheduleConfiguration : IEntityTypeConfiguration<GasStationSchedule>
    {
        public void Configure(EntityTypeBuilder<GasStationSchedule> builder)
        {
            builder.HasOne<GasStation>()
                            .WithMany()
                            .HasForeignKey(t => t.GasStationId);
            builder.HasKey(x => new { x.GasStationId, x.ScheduleType });
        }
    }
}
