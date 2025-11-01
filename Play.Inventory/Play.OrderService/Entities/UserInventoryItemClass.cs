using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Play.OrderService.Entities
{
    public class UserInventoryItemClass
    {
        [BsonRepresentation(BsonType.String)]
        public Guid catalogItemId { get; set; }
        public int quantity {  get; set; }
        public int price { get; set; }
        public DateTimeOffset InventoryCreatedDate { get; set; }
    }
}
