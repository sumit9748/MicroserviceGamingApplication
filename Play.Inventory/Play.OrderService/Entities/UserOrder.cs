using System.ComponentModel.DataAnnotations;

namespace Play.OrderService.Entities
{
    public class UserOrder
    {
        [Key]
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public double OrderTotal { get; set; }
        public List<UserInventory> InventoryItems { get; set; }
    }
}
