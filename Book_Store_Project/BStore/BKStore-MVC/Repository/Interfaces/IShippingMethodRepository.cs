using BKStore_MVC.Models;

namespace BKStore_MVC.Repository.Interfaces
{
    public interface IShippingMethodRepository
    {
        public void Add(ShippingMethod shippingMethod);
        public void Update(ShippingMethod shippingMethod);
        public void Delete(int ID);
        public List<ShippingMethod> GetAll();
        public ShippingMethod GetByID(int ID);
        public ShippingMethod GetByName(string Name);
        public void Save();

    }
}
