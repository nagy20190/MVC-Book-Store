using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BKStore_MVC.Models;

namespace BKStore_MVC.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        [StringLength(50, ErrorMessage = " Title cannot exceed 50 characters.")]
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        [StringLength(50, ErrorMessage = "Author Name cannot exceed 50 characters.")]
        [Display(Name = "Author Name")]
        public string? AuthorName { get; set; }
        [StringLength(50, ErrorMessage = " Title cannot exceed 50 characters.")]
        public string? ISBN { get; set; }
        [Range(10, 10000)]
        public double Price { get; set; }
        //public double Discount { get; set; }
        [StringLength(50, ErrorMessage = "Publisher Name cannot exceed 50 characters.")]
        [Display(Name = "Publisher Name")]
        public string? PublisherName { get; set; }
        [Required(ErrorMessage = "Stock Quantity Cannot be null")]
        public int StockQuantity { get; set; }
        [ForeignKey("Category")]
        [Required(ErrorMessage = "Choose Category")]
        public int CategoryID { get; set; }
        [StringLength(100, ErrorMessage = "ImagePath cannot exceed 100 characters.")]
        public string? ImagePath { get; set; }
        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string? Description { get; set; }
        [Display(Name = "Publish date")]
        public DateTime? Publishdate { get; set; }
        public double? discount { get; set; }
        public double AverageRating { get; set; } // New property
        public virtual ICollection<BookRating>? Ratings { get; set; }
        public Category? Category { get; set; }
        public ICollection<OrderBook>? orderDetails { get; set; }
        public ICollection<Reviews>? books { get; set; }
    }
}
