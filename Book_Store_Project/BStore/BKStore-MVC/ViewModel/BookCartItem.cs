namespace BKStore_MVC.ViewModel
{
    public class BookCartItem
    {
        public int? BookId { get; set; }
        public string? Title { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public string? ImagePath { get; set; }
        public string? SearchName { get; set; }
        public int? StockQuantity { get; set; }

    }
}
