using System.ComponentModel.DataAnnotations;

namespace BKStore_MVC.ViewModel
{
    public class LoginBS
    {
        [Required(ErrorMessage ="*")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me!!")]
        public bool RememberMe { get; set; }
    }
}
