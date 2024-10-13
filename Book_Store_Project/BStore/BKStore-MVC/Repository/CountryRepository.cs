using BKStore_MVC.Models.Context;
using BKStore_MVC.Models;
using BKStore_MVC.Repository.Interfaces;

namespace BKStore_MVC.Repository
{
    public class CountryRepository:ICountryRepository
    {
        BKstore_System context;
        public CountryRepository(BKstore_System _context)
        {
            context = _context;
        }
        public void Add(Country country)
        {
            context.Add(country);
        }

        public void Delete(int ID)
        {
            Country country = GetByID(ID);
            context.Remove(country);
        }

        public List<Country> GetAll()
        {
            return context.Country.ToList();
        }

        public Country GetByID(int ID)
        {
            return context.Country.FirstOrDefault(c => c.ID== ID) ?? new Country();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Country country)
        {
            context.Update(country);
        }
    }
}
