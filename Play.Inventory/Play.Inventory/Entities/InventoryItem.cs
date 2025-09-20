using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Play.Common;

namespace Play.Inventory.Entities
{
    public class InventoryItem : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid CatalogItemId { get; set; }

        public int Quantity { get; set; }
        public DateTimeOffset AcquiredDate { get; set; }
    }
}
