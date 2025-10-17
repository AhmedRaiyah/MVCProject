using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Session? GetById(int id);
        IEnumerable<Session> GetAll();
        IEnumerable<Session> GetAllSessionsWithTrainersAndCategory();
        Session GetSessionWithTrainersAndCategory(int sessionId);
        int GetCountOfBookedSlots(int sessionId);
        int Add(Session session);
        int Update(Session session);
        int Delete(int id);
    }
}
