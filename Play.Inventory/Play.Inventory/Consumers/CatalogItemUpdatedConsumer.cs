using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Entities;
using System.Threading.Tasks;

namespace Play.Inventory.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        private readonly IRepository<CatalogItem> _repository;

        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;
            var item = await _repository.GetAsync(message.itemId);

            if (item == null)
            {
                item = new CatalogItem
                {
                    Id = message.itemId,
                    Name = message.name,
                    Description = message.description
                };
                await _repository.CreateAsync(item);
            } else
            {
                item.Name = message.name;
                item.Description = message.description;
                await _repository.UpdateAsync(item);
            }            
        }
    }
}
