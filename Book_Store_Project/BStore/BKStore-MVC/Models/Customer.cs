using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BKStore_MVC.Models;

namespace BKStore_MVC.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }
        [StringLength(30, ErrorMessage = " name cannot exceed 30 characters.")]
        public string Name { get; set; }
        [StringLength(400, ErrorMessage = " Address cannot exceed 400 characters.")]
        public string Address { get; set; }
        [StringLength(15, ErrorMessage = " Phone cannot exceed 15 characters.")]
        public string Phone { get; set; }
        
        [Display(Name = "national number")]
        public string? Nationalnumber { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public string? UserID { get; set; }
        [ForeignKey(nameof(Governorate))]
        public int? GovernorateID { get; set; }
        public ICollection<Order>? Order { get; set; }
        public ApplicationUser? User { get; set; }
        public Governorate Governorate { get; set; }
    }
}
