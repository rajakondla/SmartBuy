using Microsoft.EntityFrameworkCore;
using SmartBuy.Administration.Domain;
using SmartBuy.Administration.Infrastructure.Configurations;

namespace SmartBuy.Administration.Infrastructure
{
    public class AdministrationContext : DbContext
    {
        public AdministrationContext()
        {

        }

        public AdministrationContext(DbContextOptions<AdministrationContext> options) : base(options)
        {

        }

        public DbSet<GasStation> GasStations { get; set; }
        public DbSet<Tank> Tanks { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<TankReading> TankReadings { get; set; }
        public DbSet<TankSale> TankSales { get; set; }
        public DbSet<OrderStrategy> OrderStrategies { get; set; }
        public DbSet<GasStationSchedule> GasStationSchedules { get; set; }
        public DbSet<GasStationScheduleByDay> GasStationScheduleByDays { get; set; }
        public DbSet<GasStationScheduleByTime> GasStationScheduleByTimes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.
                    UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog= SmartBuy ; Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Administrator");

            modelBuilder.ApplyConfiguration(new GasStationConfiguration());
            modelBuilder.ApplyConfiguration(new TankConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new TankReadingConfiguration());
            modelBuilder.ApplyConfiguration(new TankSaleConfiguration());
            modelBuilder.ApplyConfiguration(new OrderStrategyConfiguration());
            modelBuilder.ApplyConfiguration(new GasStationScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new GasStationScheduleByDayConfiguration());
            modelBuilder.ApplyConfiguration(new GasStationScheduleByTimeConfiguration());
            modelBuilder.ApplyConfiguration(new GasStationTankScheduleConfiguration());
        }
    }
}
