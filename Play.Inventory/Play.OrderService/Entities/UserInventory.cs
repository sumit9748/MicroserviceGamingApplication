
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Play.OrderService.Entities
{

    public class UserInventory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid CatalogItemId { get; set; }

        public int Quantity { get; set; }
        public int Price { get; set; }
        public DateTimeOffset AcquiredDate { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        [JsonIgnore]
        public UserOrder Order { get; set; }
    }

}
