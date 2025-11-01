using Microsoft.EntityFrameworkCore;
using Play.OrderService.Entities;

namespace Play.OrderService.DbContextClass
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserOrder>()
                .HasMany(o => o.InventoryItems)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId);
        }

        // Define your DbSets (tables) here
        public DbSet<UserInventory> UserInventories { get; set; }
        public DbSet<UserOrder> UserOrders { get; set; }
    }
}
