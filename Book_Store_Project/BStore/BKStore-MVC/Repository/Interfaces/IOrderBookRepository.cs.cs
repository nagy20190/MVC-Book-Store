using BKStore_MVC.Models;

namespace BKStore_MVC.Repository.Interfaces
{
    public interface IOrderBookRepository
    {
        public void Add(OrderBook orderBook);
        public void Update(OrderBook orderBook);
        public void Delete(int ID);
        public OrderBook GetByBookID(int ID);
        public List<OrderBook> GetAll();
        public List<OrderBook> GetByID(int ID);
        public void Save();
    }
}
