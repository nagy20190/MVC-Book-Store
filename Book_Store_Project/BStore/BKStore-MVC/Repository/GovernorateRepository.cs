using BKStore_MVC.Models.Context;
using BKStore_MVC.Models;
using BKStore_MVC.Repository.Interfaces;

namespace BKStore_MVC.Repository
{
    public class GovernorateRepository:IGovernorateRepository
    {
        BKstore_System context;
        public GovernorateRepository(BKstore_System _context)
        {
            context = _context;
        }
        public void Add(Governorate governorate)
        {
            context.Add(governorate);
        }

        public void Delete(int ID)
        {
            Governorate governorate = GetByID(ID);
            context.Remove(governorate);
        }

        public List<Governorate> GetAll()
        {
            return context.governorate.ToList();
        }

        public Governorate GetByID(int ID)
        {
            return context.governorate.FirstOrDefault(c => c.Id== ID) ?? new Governorate();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Governorate governorate)
        {
            context.Update(governorate);
        }
    }
}
