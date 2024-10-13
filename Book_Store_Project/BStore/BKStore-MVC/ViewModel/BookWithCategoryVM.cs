using BKStore_MVC.Models;

namespace BKStore_MVC.ViewModel
{
    public class BookWithCategoryVM
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }    
        public List<Book> books { get; set; }

    }
}
