using BKStore_MVC.Models;
using System.ComponentModel.DataAnnotations;

namespace BKStore_MVC.ViewModels
{
        public class RegistersRoles
        {
            //[Unique]
            public string UserName { get; set; }

            public string Email { get; set; }



            [DataType(DataType.Password)]
            public string Password { get; set; }


            [Compare(nameof(Password))]
            [Display(Name = "Confirm Password")]
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }


            [Required]
            public string Role { get; set; }


        }
}

