using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDBContext _context;

        public SessionRepository(GymDBContext context) : base(context) 
        {
            _context = context;
        }
        public int Add(Session session)
        {
            _context.Add(session);
            return _context.SaveChanges();
        }

        public int Delete(int id)
        {
            var session = GetById(id);

            if (session is null)
                return 0;

            _context.Remove(session);
            return _context.SaveChanges();
        }

        public IEnumerable<Session> GetAll() => _context.Sessions.ToList();

        public IEnumerable<Session> GetAllSessionsWithTrainersAndCategory()
        {
            return _context.Sessions
                           .Include(s => s.Trainer)
                           .Include(s => s.Category)
                           .ToList();
        }

        public Session? GetById(int id) => _context.Sessions.Find(id);

        public int GetCountOfBookedSlots(int sessionId)
        {
            return _context.Bookings.Where(x => x.SessionId == sessionId).Count();
        }

        public Session GetSessionWithTrainersAndCategory(int sessionId)
        {
            return _context.Sessions
                           .Include(s => s.Trainer)
                           .Include(s => s.Category)
                           .FirstOrDefault(x => x.Id == sessionId);
        }

        public int Update(Session session)
        {
            _context.Update(session);
            return _context.SaveChanges();
        }
    }
}
