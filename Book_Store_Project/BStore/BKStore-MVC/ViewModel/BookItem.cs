namespace BKStore_MVC.ViewModel
{
    public class BookItem
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public string Title { get; set; }
        public double price { get; set; }
        public string? SearchName { get; set; }
    }
}
