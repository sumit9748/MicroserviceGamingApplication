

using Play.Catalog.DTOS;
using Play.Catalog.Entities;

namespace Play.Catalog.Extensions
{
    public static class Extension
    {
        public static ItemDTO AsDto(this Item item)
        {
            return new ItemDTO(item.Id, item.Name, item.Description, item.Price);
        }
    }
}
