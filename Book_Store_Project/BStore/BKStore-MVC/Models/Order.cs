using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BKStore_MVC.Models;

namespace BKStore_MVC.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public double? TotalAmount { get; set; }
        [ForeignKey("Customer")]
        public int? CustomerID { get; set; }
        public string? DelivaryStatus { get; set; }
        [ForeignKey(nameof(DeliveryClients))]
        public int? DeliveryClientsID { get; set; }
        public DeliveryClients? DeliveryClients { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<OrderBook>? OrderBook { get; set; }
        public Shipping? Shipping { get; set; }
    }
}
