
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.Administration.Domain;

namespace SmartBuy.Administration.Infrastructure.Configurations
{
    public class GasStationTankScheduleConfiguration : IEntityTypeConfiguration<GasStationTankSchedule>
    {
        public void Configure(EntityTypeBuilder<GasStationTankSchedule> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne<Tank>()
                            .WithMany()
                            .HasForeignKey(t => t.TankId);
        }
    }
}
