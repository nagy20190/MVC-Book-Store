using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BKStore_MVC.Models
{
    public class Shipping
    {
        [Key]
        public int ShippingID { get; set; }
        [ForeignKey(nameof(Order))]
        public int? OrderID { get; set; }
        [ForeignKey(nameof(ShippingMethod))]
        public int? ShippingMethodID { get; set; }
        public DateTime? ShippingDate { get; set; }
        public int? TrackingNumber { get; set; }
        public Order? Order { get; set; }
        public ShippingMethod? ShippingMethod { get; set; }
    }
}
