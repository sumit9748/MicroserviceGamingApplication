using MassTransit;
using Play.Common;
using Play.Contract;
using Play.Inventory.Entities;

namespace Play.Inventory.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly IMongoRepositry<CatalogItem> _repo;
        public CatalogItemCreatedConsumer(IMongoRepositry<CatalogItem> repositry)
        {
            _repo = repositry;
        }

        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;
            var item = await _repo.GetAsync(message.ItemId);

            if (item != null)
            {
                return;
            }
            item = new CatalogItem
            {
                Id = message.ItemId,
                Name = message.Name,
                Description = message.Description,
            };

            await _repo.CreateAsync(item);
        }
    }
}
