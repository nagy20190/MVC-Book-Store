using BKStore_MVC.Models.Context;
using BKStore_MVC.Models;
using BKStore_MVC.Repository.Interfaces;

namespace BKStore_MVC.Repository
{
    public class ShippingRepository:IShippingRepository
    {
        BKstore_System context;
        public ShippingRepository(BKstore_System _context)
        {
            context = _context;
        }
        public void Add(Shipping shipping)
        {
            context.Add(shipping);
        }

        public void Delete(int ID)
        {
            Shipping shipping = GetByID(ID);
            context.Remove(shipping);
        }

        public List<Shipping> GetAll()
        {
            return context.Shipping.ToList();
        }

        public Shipping GetByID(int ID)
        {
            return context.Shipping.FirstOrDefault(c => c.ShippingID== ID) ?? new Shipping();
        }
        public Shipping GetByOrderID(int ID)
        {
            return context.Shipping.FirstOrDefault(c => c.OrderID == ID) ?? new Shipping();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Shipping shipping)
        {
            context.Update(shipping);
        }
    }
}
