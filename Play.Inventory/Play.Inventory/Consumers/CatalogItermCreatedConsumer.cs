using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Entities;
using System.Threading.Tasks;

namespace Play.Inventory.Consumers
{
    public class CatalogItermCreatedConsumer : IConsumer<CatalogItemCreated>
    {

        private readonly IRepository<CatalogItem> _repository;

        public CatalogItermCreatedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;

            var item = await _repository.GetAsync(message.itemId);
            if (item != null) return;

            item = new CatalogItem
            {
                Id = message.itemId,
                Name = message.name,
                Description = message.description
            };

            await _repository.CreateAsync(item);
        }
    }
}
