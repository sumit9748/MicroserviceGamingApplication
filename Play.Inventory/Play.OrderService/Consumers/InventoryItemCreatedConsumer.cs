

using MassTransit;
using MassTransit.Initializers;
using Play.Common;
using Play.Contract;
using Play.OrderService.Entities;

namespace Play.OrderService.Consumers
{
    public class InventoryItemCreatedConsumer : IConsumer<InventoryItemCreated>
    {
        private readonly IMongoRepositry<UserBucket> _userBucketRepo;
        public InventoryItemCreatedConsumer(IMongoRepositry<UserBucket> userBucketRepo)
        {
            _userBucketRepo = userBucketRepo;
        }

        public async Task Consume(ConsumeContext<InventoryItemCreated> context)
        {
            try
            {
                var message = context.Message;
                var userinventories = message.inventoryList.ToList();
                var userId = message.UserId;

                List<UserInventoryItemClass> userInventoryList = new();
                var userBucketList = await _userBucketRepo.GetAsync(u => u.UserId == userId);
                
                foreach(var  item in userinventories)
                {
                    userInventoryList.Add(new UserInventoryItemClass
                    {
                        catalogItemId = item.catalogItemId,
                        quantity = item.quantity,
                        price = item.price,
                        InventoryCreatedDate = DateTime.UtcNow
                    });
                }
                var userBucket = new UserBucket
                {
                    UserId = userId,
                    inventoryItems = userInventoryList

                };
                if (userBucketList != null)
                {
                    await _userBucketRepo.DeleteAsync(userBucketList.Id);
                }
                await _userBucketRepo.CreateAsync(userBucket);
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            
        }
    }
}
