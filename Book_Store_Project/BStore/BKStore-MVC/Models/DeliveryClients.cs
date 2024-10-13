using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKStore_MVC.Models
{
    public class DeliveryClients
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        [Display(Name = "National ID")]
        [Required]
        [StringLength(50)]
        public string NationalID  { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        [Required]
        public string UserID { get; set; }
        public bool IsLocked { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ReviewsDelivery ReviewsDelivery { get; set; }

    }
}
