using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Entities;

namespace Play.Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IMongoRepositry<InventoryItem> _repositry;
        private readonly IMongoRepositry<CatalogItem> _repo;
        public InventoryController(IMongoRepositry<InventoryItem> repositry, IMongoRepositry<CatalogItem> repo)
        {
            _repositry = repositry;
            _repo = repo;
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
            var inventoryItem = await _repositry.GetAsync(item => item.UserId == grantItemsDto.UserId
            && item.CatalogItemId == grantItemsDto.CatalogItemId);
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

        }
    }
}
