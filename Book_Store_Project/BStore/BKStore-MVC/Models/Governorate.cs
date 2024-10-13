using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKStore_MVC.Models
{
    public class Governorate
    {
        public int Id { get; set; }
        [StringLength(30, ErrorMessage = " name cannot exceed 30 characters.")]
        public string Name { get; set; }
        [ForeignKey(nameof(Country))]
        public int? CountryID { get; set; }
        public ICollection<Customer>? customers { get; set; }
        public Country? Country { get; set; }
        public ICollection<ShippingMethod>? shippingMethod { get; set; }
    }
}
