using BKStore_MVC.Models;

namespace BKStore_MVC.Repository.Interfaces
{
    public interface IDeliveryClientRepository
    {
        public void Add(DeliveryClients deliveryClients);
        public void Update(DeliveryClients deliveryClients);
        public void Delete(int ID);
        public DeliveryClients GetByUserID(string ID);
        public List<DeliveryClients> GetAll();
        public DeliveryClients GetByID(int ID);
        public void Save();

    }
}
