using BKStore_MVC.Models.Context;
using BKStore_MVC.Models;
using BKStore_MVC.Repository.Interfaces;


namespace BKStore_MVC.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        BKstore_System context;
        public CustomerRepository(BKstore_System _context)
        {
            context = _context;
        }
        public void Add(Customer customer)
        {
            context.Add(customer);
        }

        public void Delete(int ID)
        {
            Customer customer = GetByID(ID);
            context.Remove(customer);
        }

        public List<Customer> GetAll()
        {
            return context.Customer.ToList();
        }

        public Customer GetByID(int ID)
        {
            return context.Customer.FirstOrDefault(c => c.ID == ID) ?? new Customer();
        }
        public Customer GetByUserID(string ID)
        {
            return context.Customer.FirstOrDefault(c => c.UserID == ID) ?? new Customer();
        }
        public Customer GetByName(string name)
        {
            return context.Customer.FirstOrDefault(c => c.Name == name) ?? new Customer();
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Customer customer)
        {
            context.Update(customer);
        }
    }
}
