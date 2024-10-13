using BKStore_MVC.Models;

namespace BKStore_MVC.Repository.Interfaces
{
    public interface IGovernorateRepository
    {
        public void Add(Governorate governorate);
        public void Update(Governorate governorate);
        public void Delete(int ID);
        public List<Governorate> GetAll();
        public Governorate GetByID(int ID);
        public void Save();
    }
}
