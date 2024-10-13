using BKStore_MVC.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BKStore_MVC.Models
{
    public class ApplicationUser:IdentityUser
    {
        [StringLength(100, ErrorMessage = "ImagePath cannot exceed 100 characters.")]
        public string? ImagePath { get; set; }
        public Reviews Reviews { get; set; }
        public Customer Customer { get; set; }
    }
}
