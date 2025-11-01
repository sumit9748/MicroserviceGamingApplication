using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Contract;
using Play.Inventory.Entities;

namespace Play.Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IMongoRepositry<InventoryItem> _repositry;
        private readonly IMongoRepositry<CatalogItem> _repo;
        private readonly IPublishEndpoint publishEndpoint;
        public InventoryController(IMongoRepositry<InventoryItem> repositry, IMongoRepositry<CatalogItem> repo,
            IPublishEndpoint _publishEndpoint)
        {
            _repositry = repositry;
            _repo = repo;
            publishEndpoint = _publishEndpoint;
        }
        [HttpGet("{UserId}")]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAllAync(Guid UserId)
        {
            if (UserId == Guid.Empty)
            {
                return BadRequest();
            }
            var inventoryItemEntities = await _repositry.GetAllAsync(item => item.UserId == UserId);
            var itemIds = inventoryItemEntities.Select(i => i.CatalogItemId);
            var catalogItemDtos = await _repo.GetAllAsync(item => itemIds.Contains(item.Id));

            var ans = inventoryItemEntities.Select(invenoryItem =>
            {
                var catalogItem = catalogItemDtos.Single(catalogItem => catalogItem.Id == invenoryItem.CatalogItemId);
                return invenoryItem.AsDto(catalogItem.Name, catalogItem.Description);
            });
            

            return Ok(ans);
        }


        [HttpPost]
        public async Task PostAsync([FromBody] GrantItemsDto grantItemsDto)
        {
            var userItems = await _repositry.GetAllAsync(item => item.UserId == grantItemsDto.UserId);
            var inventoryItem= userItems.FirstOrDefault(f=>f.CatalogItemId==grantItemsDto.CatalogItemId);

            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem
                {
                    CatalogItemId = grantItemsDto.CatalogItemId,
                    UserId = grantItemsDto.UserId,
                    Quantity = grantItemsDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow,
                };
                await _repositry.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += grantItemsDto.Quantity;
                await _repositry.UpdateAsync(inventoryItem);
            }
            userItems = await _repositry.GetAllAsync(item => item.UserId == grantItemsDto.UserId);
            var catalogItemIds=userItems.Select(item => item.CatalogItemId).ToList();
            var catalogItems = await _repo.GetAllAsync(item => catalogItemIds.Contains(item.Id));
            List<UserInventoryItem> items = new();
            foreach (var item in userItems)
            {
                var userInventoryItem = new UserInventoryItem(item.CatalogItemId, item.Quantity,
                    catalogItems.FirstOrDefault(i => i.Id == item.CatalogItemId).Price, DateTimeOffset.UtcNow);
                items.Add(userInventoryItem);

            }
            await publishEndpoint.Publish(new InventoryItemCreated(inventoryItem.UserId, items));

        }
    }
}
