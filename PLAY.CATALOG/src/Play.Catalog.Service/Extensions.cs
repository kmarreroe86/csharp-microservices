using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item itemEntity)
        {
            var dto = new ItemDto
            (
                itemEntity.Id,
                itemEntity.Name,
                itemEntity.Description,
                itemEntity.Price,
                itemEntity.CreatedDate
            );

            return dto;
        }
    }
}
