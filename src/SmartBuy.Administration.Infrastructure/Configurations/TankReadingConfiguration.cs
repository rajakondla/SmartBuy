using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.Administration.Domain;

namespace SmartBuy.Administration.Infrastructure.Configurations
{
    public class TankReadingConfiguration : IEntityTypeConfiguration<TankReading>
    {
        public void Configure(EntityTypeBuilder<TankReading> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasOne<Tank>()
                            .WithMany()
                            .HasForeignKey(t => t.TankId);
        }
    }
}
