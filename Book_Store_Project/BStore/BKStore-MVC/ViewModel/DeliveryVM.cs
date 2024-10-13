using System.ComponentModel.DataAnnotations;

namespace BKStore_MVC.ViewModel
{
    public class DeliveryVM
    {
        public string? UserID { get; set; }
        public string? FullName { get; set; }
        public string? NationalID { get; set; }
        public bool IsLocked { get; set; }
        public string? UserName { get; set; }

        public string? Email { get; set; }



        [DataType(DataType.Password)]
        public string? Password { get; set; }


        [Compare(nameof(Password))]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
        public string? Phone {  get; set; }

    }
}
