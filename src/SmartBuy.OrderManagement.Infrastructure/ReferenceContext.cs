using Microsoft.EntityFrameworkCore;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Infrastructure.Configurations;

namespace SmartBuy.OrderManagement.Infrastructure
{
    public class ReferenceContext : DbContext
    {
        public ReferenceContext()
        {

        }

        public ReferenceContext(DbContextOptions<ReferenceContext> options) : base(options)
        {
        }

        public DbSet<GasStation> GasStations { get; set; }
        public DbSet<Tank> Tanks { get; set; }
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
            modelBuilder.ApplyConfiguration(new TankConfiguration());
            modelBuilder.Entity<TankSale>(x => x.HasNoKey());
            modelBuilder.Entity<TankReading>(x => x.HasNoKey());
            modelBuilder.Entity<GasStationSchedule>(x => x.HasNoKey());
            modelBuilder.Entity<GasStationScheduleByDay>(x => x.HasNoKey());
            modelBuilder.Entity<GasStationScheduleByTime>(x => x.HasNoKey());
            modelBuilder.Entity<OrderStrategy>(x => x.HasNoKey());
            modelBuilder.Entity<GasStationTankSchedule>(x => x.HasNoKey());
        }
    }
}
