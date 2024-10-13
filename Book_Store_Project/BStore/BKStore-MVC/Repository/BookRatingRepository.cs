using BKStore_MVC.Models;
using BKStore_MVC.Models.Context;
using BKStore_MVC.Repository.Interfaces;

namespace BKStore_MVC.Repository
{
    public class BookRatingRepository:IBookRatingRepository
    {
        BKstore_System context;
        public BookRatingRepository(BKstore_System _context)
        {
            context = _context;
        }

        public List<BookRating> GetAll()
        {
            return context.BookRatings.ToList();

        }
        public List<BookRating> GetByBookID(int id)
        {
            return context.BookRatings.Where(b=>b.BookID == id).ToList();
        }


    }
}
