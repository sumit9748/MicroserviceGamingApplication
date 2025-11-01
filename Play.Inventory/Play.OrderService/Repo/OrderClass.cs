

using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using Play.Common;
using Play.OrderService.DbContextClass;
using Play.OrderService.Entities;
using System.Linq;
using System.Linq.Expressions;

namespace Play.OrderService.Repo
{
    public class OrderClass : IOrderClass
    {
        private readonly IMongoRepositry<UserBucket> userBuckets;
        
        private readonly OrderDbContext db;
        public OrderClass(IMongoRepositry<UserBucket> _userBuckets,OrderDbContext _db)
        {
            userBuckets = _userBuckets;
            db = _db;
        }
        public async Task CreateOrder(Guid userId)
        {
            using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                var orderId = Guid.NewGuid();

                var buckets = await userBuckets.GetAsync(u => u.UserId == userId);

                var order = new UserOrder
                {
                    OrderId = orderId,
                    UserId = userId,
                    OrderTotal = buckets.inventoryItems.Sum(i => i.price)
                };

                foreach (var item in buckets.inventoryItems)
                {
                    order.InventoryItems.Add(new UserInventory
                    {
                        OrderId = orderId,
                        UserId = userId,
                        CatalogItemId = item.catalogItemId,
                        Price = item.price,
                        Quantity = item.quantity
                    });
                }

                await db.Set<UserOrder>().AddAsync(order);
                await db.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        public async Task<UserOrder> GetUserOrder(Guid userId)
        {
            IQueryable<UserOrder> userOrder =  db.Set<UserOrder>();
            userOrder = userOrder.Include(u=>u.InventoryItems)
                .Where(u => u.UserId == userId);

            return userOrder.FirstOrDefault();
        }
    }
}
