using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Play.Common;

namespace Play.Inventory.Entities
{
    public class CatalogItem : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}
