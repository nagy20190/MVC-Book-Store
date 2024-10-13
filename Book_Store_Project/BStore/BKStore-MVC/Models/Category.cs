using System.ComponentModel.DataAnnotations;

namespace BKStore_MVC.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [StringLength(30, ErrorMessage = " name cannot exceed 30 characters.")]
        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }
        [StringLength(300, ErrorMessage = " name cannot exceed 300 characters.")]
        [Required(ErrorMessage = "Please Enter Description")]
        public string Description { get; set; }
        public ICollection<Book>? Book {  get; set; }
    }
}
