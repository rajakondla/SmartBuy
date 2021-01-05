using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.OrderManagement.Domain;

namespace SmartBuy.OrderManagement.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Comments).HasMaxLength(200);
            builder.OwnsOne(o => o.DispatchDate).
                Property(o => o.Start)
                .HasColumnName("FromDateTime");
            builder.OwnsOne(o => o.DispatchDate).
                Property(o => o.End)
                .HasColumnName("ToDateTime");
            builder.Ignore(o => o.State);
            builder.Ignore(o => o.IsConflicting);
            //builder.HasOne<GasStation>()
            //       .WithMany()
            //       .HasForeignKey(o => o.GasStationId);
        }
    }
}
