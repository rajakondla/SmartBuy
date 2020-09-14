using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.Administration.Domain;

namespace SmartBuy.Administration.Infrastructure.Configurations
{
    public class DispatcherConfiguration : IEntityTypeConfiguration<Dispatcher>
    {
        public void Configure(EntityTypeBuilder<Dispatcher> builder)
        {
            builder.HasKey(g => g.Id);
            builder.HasOne<DispatcherGroup>()
                       .WithMany()
                       .HasForeignKey(t => t.DispatcherGroupId);
        }
    }
}
