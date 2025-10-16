using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GymManagementDAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDBContext _context;
        private readonly Dictionary<string, object> repositories = [];

        public UnitOfWork(GymDBContext context)
        {
            _context = context;
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            var entityName = typeof(TEntity).Name;

            if (repositories.TryGetValue(entityName, out object? value))
                return (IGenericRepository<TEntity>)value;

            var repositiry = new GenericRepository<TEntity>(_context);

            repositories.Add(entityName, repositiry);

            return repositiry;
        }

        public int SaveChanges() => _context.SaveChanges();
    }
}
