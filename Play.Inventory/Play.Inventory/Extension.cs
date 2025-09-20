

using Play.Inventory.Entities;

namespace Play.Inventory
{
    public static class Extension
    {
        public static InventoryItemDto AsDto(this InventoryItem item,string Name,string Description)
        {
            return new InventoryItemDto(item.CatalogItemId,Name,Description, item.Quantity, item.AcquiredDate);
        }
    }
}
