using BKStore_MVC.Models.Context;
using BKStore_MVC.Models;
using BKStore_MVC.Repository.Interfaces;

namespace BKStore_MVC.Repository
{
    public class CategoryRepository:ICategoryRepository
    {
        BKstore_System context;
        public CategoryRepository(BKstore_System _context)
        {
            context = _context;
        }
        public void Add(Category category)
        {
            context.Add(category);
        }

        public void Delete(int ID)
        {
            Category category = GetByID(ID);
            context.Remove(category);
        }

        public List<Category> GetAll()
        {
            return context.Category.ToList();
        }

        public Category GetByID(int ID)
        {
            return context.Category.FirstOrDefault(c => c.CategoryID== ID);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Category category)
        {
            context.Update(category);
        }
    }
}
