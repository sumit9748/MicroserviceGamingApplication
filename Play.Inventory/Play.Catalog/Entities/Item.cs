using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Play.Common;

namespace Play.Catalog.Entities
{
    public class Item: IEntity
    {
        // ✅ Convert ObjectId to string automatically
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set;}
    }
}
