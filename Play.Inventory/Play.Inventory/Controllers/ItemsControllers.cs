using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Clients;
using Play.Inventory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> invetoryItemsRepository;

        private readonly IRepository<CatalogItem> catalogItemsRepository;

        public ItemsController(IRepository<InventoryItem> invetoryItemsRepository, IRepository<CatalogItem> catalogItemsRepository)
        {
            this.invetoryItemsRepository = invetoryItemsRepository;
            this.catalogItemsRepository = catalogItemsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            if (userId == null) return BadRequest();
                        
            var inventoryItemEntities = await invetoryItemsRepository.GetAllAsync(item => item.UserId == userId);
            var itemIds = inventoryItemEntities.Select(item => item.CatalogItemId).ToList();
            var catalogItemEntities = await catalogItemsRepository.GetAllAsync(item => itemIds.Contains(item.Id));

            var inventoryItemDtos = inventoryItemEntities.Select(inventoryItem => {
                    var catalogItem = catalogItemEntities.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
                return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
                });

            return Ok(inventoryItemDtos);
        }

        [HttpPost]
        public async Task<ActionResult<InventoryItemDto>> PostAsync([FromBody] GrantItemsDto grantItemsDto)
        {
            var existingInventory = await invetoryItemsRepository.GetAsync(inv =>
                        inv.UserId == grantItemsDto.UserId && inv.CatalogItemId == grantItemsDto.CatalogItemId);

            if (existingInventory == null)
            {
                var inventoryItemEntity = new InventoryItem
                {
                    Id = Guid.NewGuid(),
                    UserId = grantItemsDto.UserId,
                    CatalogItemId = grantItemsDto.CatalogItemId,
                    Quantity = grantItemsDto.Quantity,
                    AcquiredDate = DateTimeOffset.Now
                };

                await invetoryItemsRepository.CreateAsync(inventoryItemEntity);

            }
            else
            {
                existingInventory.Quantity += grantItemsDto.Quantity;
                await invetoryItemsRepository.UpdateAsync(existingInventory);
            }

            return Ok();
        }

    }
}
