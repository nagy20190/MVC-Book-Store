using BKStore_MVC.Models.Context;
using BKStore_MVC.Models;
using BKStore_MVC.Repository.Interfaces;

namespace BKStore_MVC.Repository
{
    public class ShippingMethodRepository : IShippingMethodRepository
    {
        BKstore_System context;
        public ShippingMethodRepository(BKstore_System _context)
        {
            context = _context;
        }
        public void Add(ShippingMethod shippingMethod)
        {
            context.Add(shippingMethod);
        }

        public void Delete(int ID)
        {
            ShippingMethod shipping = GetByID(ID);
            context.Remove(shipping);
        }

        public List<ShippingMethod> GetAll()
        {
            return context.shippingMethod.ToList();
        }

        public ShippingMethod GetByID(int ID)
        {
            return context.shippingMethod.FirstOrDefault(c => c.Id == ID) ?? new ShippingMethod();
        }
        public ShippingMethod GetByName(string Name)
        {
            return context.shippingMethod.FirstOrDefault(s=>s.Name==Name)??new ShippingMethod();
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(ShippingMethod shipping)
        {
            context.Update(shipping);
        }
    }
}
