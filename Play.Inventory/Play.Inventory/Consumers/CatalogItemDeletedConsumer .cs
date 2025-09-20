using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Entities;

namespace Play.Inventory.Consumers
{
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IMongoRepositry<CatalogItem> _repo;
        public CatalogItemDeletedConsumer(IMongoRepositry<CatalogItem> repositry)
        {
            _repo = repositry;
        }

        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            var message = context.Message;
            await _repo.DeleteAsync(message.ItemId);
        }
    }
}
