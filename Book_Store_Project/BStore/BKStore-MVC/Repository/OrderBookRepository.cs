using BKStore_MVC.Models.Context;
using BKStore_MVC.Models;
using BKStore_MVC.Repository.Interfaces;

namespace BKStore_MVC.Repository
{
    public class OrderBookRepository: IOrderBookRepository
    {
            BKstore_System context;
            public OrderBookRepository(BKstore_System _context)
            {
                context = _context;
            }
            public void Add(OrderBook orderBook)
            {
                context.Add(orderBook);
            }

            public void Delete(int ID)
            {
                List<OrderBook> orderBook = GetByID(ID);
                context.Remove(orderBook);
            }

            public List<OrderBook> GetAll()
            {
                return context.OrderBook.ToList();
            }

            public List<OrderBook> GetByID(int ID)
            {
                return context.OrderBook.Where(c => c.OrderID == ID).ToList();
            }
            public OrderBook GetByBookID(int ID)
            {
                return context.OrderBook.FirstOrDefault(c => c.BookID == ID) ?? new OrderBook();
            }
        

            public void Save()
            {
                context.SaveChanges();
            }

            public void Update(OrderBook orderBook)
            {
                context.Update(orderBook);
            }
        }
    }
