using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Infrastructure.Configurations;
using Serilog.Extensions.Logging.File;

namespace SmartBuy.OrderManagement.Infrastructure
{
    public class OrderContext : DbContext
    {
        public OrderContext()
        {

        }

        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLoggerFactory(ConsoleLoggerFactory)
                    .EnableSensitiveDataLogging()
                    .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog= SmartBuy ; Integrated Security=True;");
            }
        }

        public static readonly ILoggerFactory ConsoleLoggerFactory
  = LoggerFactory.Create(builder =>
  {
      builder
           .AddFilter((category, level) =>
               category == DbLoggerCategory.Database.Command.Name
               && level == LogLevel.Information)
           .AddFile(@"D:\Logs\query.txt");
  });

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("OrderManagement");

            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderProductConfiguration());
        }
    }
}
