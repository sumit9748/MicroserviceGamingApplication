using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Play.Contract;
using Play.Common;

namespace Play.OrderService.Entities
{
    public class UserBucket:IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }
        public List<UserInventoryItemClass> inventoryItems { get; set; }
    }
}
