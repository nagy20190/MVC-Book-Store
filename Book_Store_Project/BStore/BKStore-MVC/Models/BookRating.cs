using System.ComponentModel.DataAnnotations;

namespace BKStore_MVC.Models
{
    public class BookRating
    {
        [Key]
        public int BookRatingID { get; set; }
        public int BookID { get; set; }
        public string? UserID { get; set; } 
        public int Rating { get; set; }
        public virtual Book? Book { get; set; }
    }
}
