using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.Administration.Domain;

namespace SmartBuy.Administration.Infrastructure.Configurations
{
    public class TerminalConfiguration : IEntityTypeConfiguration<Terminal>
    {
        public void Configure(EntityTypeBuilder<Terminal> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
