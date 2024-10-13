using BKStore_MVC.Models.Context;
using BKStore_MVC.Models;
using BKStore_MVC.Repository.Interfaces;

namespace BKStore_MVC.Repository
{
    public class ReviewsRepository:IReviewsRepository
    {
        BKstore_System context;
        public ReviewsRepository(BKstore_System _context)
        {
            context = _context;
        }
        public void Add(Reviews reviews)
        {
            context.Add(reviews);
        }

        public void Delete(int ID)
        {
            Reviews reviews = GetByID(ID);
            context.Remove(reviews);
        }

        public List<Reviews> GetAll()
        {
            return context.Reviews.ToList();
        }

        public Reviews GetByID(int ID)
        {
            return context.Reviews.FirstOrDefault(c => c.ReviewId == ID) ?? new Reviews();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Reviews reviews)
        {
            context.Update(reviews);
        }
    }
}
