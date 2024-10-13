using BKStore_MVC.Models;

namespace BKStore_MVC.Repository.Interfaces
{
    public interface IReviewsRepository
    {
        public void Add(Reviews reviews);
        public void Update(Reviews reviews);
        public void Delete(int ID);
        public List<Reviews> GetAll();
        public Reviews GetByID(int ID);
        public void Save();
    }
}
