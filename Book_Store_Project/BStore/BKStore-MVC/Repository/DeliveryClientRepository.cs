using BKStore_MVC.Models.Context;
using BKStore_MVC.Models;
using BKStore_MVC.Repository.Interfaces;

namespace BKStore_MVC.Repository
{
    public class DeliveryClientRepository:IDeliveryClientRepository
    {
        BKstore_System context;
        public DeliveryClientRepository(BKstore_System _context)
        {
            context = _context;
        }
        public void Add(DeliveryClients deliveryClients)
        {
            context.Add(deliveryClients);
        }

        public void Delete(int ID)
        {
            DeliveryClients deliveryClients = GetByID(ID);
            context.Remove(deliveryClients);
        }

        public List<DeliveryClients> GetAll()
        {
            return context.DeliveryClients.ToList();
        }

        public DeliveryClients GetByID(int ID)
        {
            return context.DeliveryClients.FirstOrDefault(c => c.ID == ID)??new DeliveryClients();
        }
        public DeliveryClients GetByUserID(string ID)
        {
            return context.DeliveryClients.FirstOrDefault(c => c.UserID == ID) ?? new DeliveryClients();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(DeliveryClients deliveryClients)
        {
            context.Update(deliveryClients);
        }
    }
}
