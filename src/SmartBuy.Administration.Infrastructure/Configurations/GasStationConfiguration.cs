using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBuy.Administration.Domain;
using System;


namespace SmartBuy.Administration.Infrastructure.Configurations
{
    public class GasStationConfiguration : IEntityTypeConfiguration<GasStation>
    {
        public void Configure(EntityTypeBuilder<GasStation> builder)
        {
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Name).HasMaxLength(50);
            builder.Property(g => g.Address).HasMaxLength(200);
        }
    }
}
