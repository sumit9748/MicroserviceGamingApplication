using MassTransit;
using Play.Common;
using Play.Contract;
using Play.Inventory.Entities;

namespace Play.Inventory.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        private readonly IMongoRepositry<CatalogItem> _repo;
        public CatalogItemUpdatedConsumer(IMongoRepositry<CatalogItem> repositry)
        {
            _repo = repositry;
        }

        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;
            var item = await _repo.GetAsync(message.ItemId);

            if (item == null)
            {
                item = new CatalogItem
                {
                    Id = message.ItemId,
                    Name = message.Name,
                    Description = message.Description,
                    Price = message.Price
                };
                await _repo.CreateAsync(item);
            }
            else
            {
                item.Name = message.Name;
                item.Description = message.Description;
                item.Price = message.Price;

                await _repo.UpdateAsync(item);
            }

        }
    }
}
