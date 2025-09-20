
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.DTOS;
using Play.Catalog.Entities;
using Play.Catalog.Extensions;
using Play.Common;
using Play.Contract;
using System.Collections;

namespace Play.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IMongoRepositry<Item> _repo;
        private readonly IPublishEndpoint publishEndpoint;
        public ItemsController(IMongoRepositry<Item> repo,IPublishEndpoint _publishEndpoint)
        {
            _repo = repo;
            publishEndpoint = _publishEndpoint;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            
            var ans = (await _repo.GetAllAsync()).Select(item=>item.AsDto());
            return Ok(ans);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item=await _repo.GetAsync(id);
            return Ok(item.AsDto());
        }
        [HttpPost]
        public async Task Create([FromBody]ItemDtoReq item)
        {
            var real = new Item
            {
                Id = Guid.NewGuid(),
                Name = item.Name,
                Price = item.Price,
                Description = item.Description,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _repo.CreateAsync(real);
            await publishEndpoint.Publish(new CatalogItemCreated(real.Id, real.Name, real.Description));
        }
        [HttpPut("{id}")]
        public async Task Update(Guid id,[FromBody]ItemDTO item)
        {
            var real = new Item
            {
                Id = id,
                Name = item.Name,
                Price = item.Price,
                Description = item.Description,
                CreatedDate = DateTime.Now,
            };
            await _repo.UpdateAsync(real);
            await publishEndpoint.Publish(new CatalogItemUpdated(real.Id, real.Name, real.Description));
        }
        [HttpDelete]
        public async Task Delete(Guid id)
        {
            await _repo.DeleteAsync(id);
            await publishEndpoint.Publish(new CatalogItemDeleted(id));
        }

    }
}
