using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKStore_MVC.Models
{
    public class ReviewsDelivery
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(DeliveryClients))]
        public int DeliveryID { get; set; }
        [ForeignKey(nameof(Customer))]
        public int CustomerID { get; set; }
        [StringLength(500, ErrorMessage = " Review cannot exceed 500 characters.")]
        public string? ReviewText { get; set; }
        [Range(0, 5)]
        public decimal Rating { get; set; }
        public DeliveryClients? DeliveryClients { get; set; }
        public Customer? Customer { get; set; }
    }
}
