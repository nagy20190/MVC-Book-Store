using System.ComponentModel.DataAnnotations;

namespace BKStore_MVC.ViewModels
{
    public class RoleViewModel
    {
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
