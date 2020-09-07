
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.Administration.Domain;

namespace SmartBuy.Administration.Infrastructure.Configurations
{
    public class TankSaleConfiguration : IEntityTypeConfiguration<TankSale>
    {
        public void Configure(EntityTypeBuilder<TankSale> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasOne<Tank>()
                            .WithMany()
                            .HasForeignKey(t => t.TankId);
        }
    }
}
