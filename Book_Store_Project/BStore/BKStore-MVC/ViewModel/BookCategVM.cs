using BKStore_MVC.Models;
using X.PagedList;
namespace BKStore_MVC.ViewModel
{
    public class BookCategVM
    {
        public IEnumerable<Category> categories { get; set; }
        public IPagedList<Book> books { get; set; }
        public string? SearchName { get; set; }

    }
}
