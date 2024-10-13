using BKStore_MVC.Models;
using System.ComponentModel.DataAnnotations;

namespace BKStore_MVC.ViewModel
{
    public class RegisterBS
    {
        //[Unique]
        [Display(Name = "Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must contain only letters in English.")]
        public string UserName { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}