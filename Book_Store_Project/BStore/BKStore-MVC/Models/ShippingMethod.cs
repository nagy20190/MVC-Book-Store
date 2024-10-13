using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKStore_MVC.Models
{
    public class ShippingMethod
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "Cash on delivery";
        [ForeignKey(nameof(Governorate))]
        public int? GovernorateID { get; set; }

        [Display(Name = "Payment Fees")]
        public double? PaymentFees { get; set; }

        public ICollection<Shipping>? shippings { get; set; }
    }
}
