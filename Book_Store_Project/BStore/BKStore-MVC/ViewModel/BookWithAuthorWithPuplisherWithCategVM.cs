using BKStore_MVC.Models;

namespace BKStore_MVC.ViewModel
{
    public class BookWithAuthorWithPuplisherWithCategVM
    {
        public int BookID { get; set; }
        public string? Title { get; set; }
        public int StockQuantity { get; set; }
        public int Quantity {  get; set; }
        public double Price { get; set; }
        public int CategoryID { get; set; }
        public string? BookImagePath { get; set; }
        public string? Description { get; set; }
        public string? AuthorName { get; set; }
        public string? PublisherName { get; set; }
        public string? CategoryName { get; set; }
        public DateTime? Publishdate { get; set; }
        public string? SearchName { get; set; }
        public IFormFile? ImagePath { get; set; }
        public List<Category>? categories { get; set; }

    }
}
