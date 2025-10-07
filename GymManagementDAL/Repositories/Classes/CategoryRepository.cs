using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class CategoryRepository
    {
        private readonly GymDBContext _context;

        public CategoryRepository(GymDBContext context)
        {
            _context = context;
        }
        public int Add(Category category)
        {
            _context.Add(category);
            return _context.SaveChanges();
        }

        public int Delete(int id)
        {
            var category = GetById(id);

            if (category is null)
                return 0;

            _context.Remove(category);
            return _context.SaveChanges();
        }

        public IEnumerable<Category> GetAll() => _context.Categories.ToList();

        public Category? GetById(int id) => _context.Categories.Find(id);

        public int Update(Category category)
        {
            _context.Update(category);
            return _context.SaveChanges();
        }
    }
}
