using BKStore_MVC.Models;

namespace BKStore_MVC.ViewModel
{
    public class BookViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}
