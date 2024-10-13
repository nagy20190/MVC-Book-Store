using BKStore_MVC.Models.Context;
using BKStore_MVC.Models;
using BKStore_MVC.Repository.Interfaces;

namespace BKStore_MVC.Repository
{
    public class OrderRepository : IOrderRepository
    {
        BKstore_System context;
        public OrderRepository(BKstore_System _context)
        {
            context = _context;
        }
        public void Add(Order order)
        {
            context.Add(order);
        }

        public void Delete(int ID)
        {
            Order order = GetByID(ID);
            context.Remove(order);
        }

        public List<Order> GetAll()
        {
            return context.Order.ToList();
        }

        public Order GetByID(int ID)
        {
            return context.Order.FirstOrDefault(c => c.OrderId == ID) ?? new Order();
        }
        public Order GetByCustomerID(int ID)
        {
            return context.Order.FirstOrDefault(c => c.CustomerID == ID) ?? new Order();
        }
        public List<Order> GetByCustomersID(int ID)
        {
            return context.Order.Where(c => c.CustomerID == ID).ToList();
        }
        public Order GetBydeliveryID(int deliveryID, int OrderID)
        {
            return context.Order.FirstOrDefault(o => o.DeliveryClientsID == deliveryID && o.OrderId == OrderID) ?? new Order();
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Order order)
        {
            context.Update(order);
        }
    }
}
