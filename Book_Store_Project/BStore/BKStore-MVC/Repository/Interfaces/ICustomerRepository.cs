using BKStore_MVC.Models;

namespace BKStore_MVC.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        public void Add(Customer customer);
        public void Update(Customer customer);
        public void Delete(int ID);
        public Customer GetByUserID(string ID);
        public Customer GetByName(string name);
        public List<Customer> GetAll();
        public Customer GetByID(int ID);
        public void Save();
    }
}
