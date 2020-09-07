using Microsoft.EntityFrameworkCore;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Infrastructure.Configurations;
using System;

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
                optionsBuilder.
                    UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog= SmartBuy ; Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("OrderManagement");

            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderProductConfiguration());
        }
    }
}
