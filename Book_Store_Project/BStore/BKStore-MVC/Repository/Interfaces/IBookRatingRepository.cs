using BKStore_MVC.Models;

namespace BKStore_MVC.Repository.Interfaces
{
    public interface IBookRatingRepository
    {
        public List<BookRating> GetAll();
        public List<BookRating> GetByBookID(int id);
    }
}
