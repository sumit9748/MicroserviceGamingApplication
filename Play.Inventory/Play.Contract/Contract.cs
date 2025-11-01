

namespace Play.Contract
{
    public record UserInventoryItem(Guid catalogItemId, int quantity,int price, DateTimeOffset InventoryCreatedDate);

    public record CatalogItemCreated(Guid ItemId,string Name,string Description,int Price);

    public record CatalogItemUpdated(Guid ItemId, string Name, string Description,int Price);

    public record CatalogItemDeleted(Guid ItemId);

    public record InventoryItemCreated(Guid UserId,List<UserInventoryItem>inventoryList);

    public record InventoryItemUpdated(Guid UserId,List<UserInventoryItem> inventoryList);
}
