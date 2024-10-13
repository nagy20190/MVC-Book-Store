using BKStore_MVC.Models;

namespace BKStore_MVC.Repository.Interfaces
{
    public interface IShippingRepository
    {
        public void Add(Shipping shipping);
        public void Update(Shipping shipping);
        public void Delete(int ID);
        public List<Shipping> GetAll();
        public Shipping GetByID(int ID);
        public Shipping GetByOrderID(int ID);
        public void Save();
    }
}
