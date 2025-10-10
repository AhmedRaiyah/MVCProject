using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly GymDBContext _context;
        public GenericRepository(GymDBContext context)
        {
            _context = context;
        }
        public void Add(TEntity entity) => _context.Add(entity);

        public void Delete(TEntity entity) => _context.Remove(entity);

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
            if (condition is not null)
                return _context.Set<TEntity>().AsNoTracking().Where(condition).ToList();
            else
                return _context.Set<TEntity>().AsNoTracking().ToList();
        }

        public TEntity? GetById(int id) => _context.Set<TEntity>().Find(id);

        public void Update(TEntity entity) => _context.Update(entity);
    }
}
