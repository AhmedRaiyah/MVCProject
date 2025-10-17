using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.SesssionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; 

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel input)
        {
            if (!IsTrainerExist(input.TrainerId))
                return false;

            if (!IsCategoryExist(input.CategoryId))
                return false;

            if (!IsValidDateRange(input.StartDate, input.EndDate))
                return false;

            var session = _mapper.Map<CreateSessionViewModel, Session>(input);

            _unitOfWork.GetRepository<Session>().Add(session);
            
            return _unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = _unitOfWork.SessionRepository
                                      .GetAllSessionsWithTrainersAndCategory()
                                      .OrderByDescending(x => x.StartDate);

            if (sessions == null || !sessions.Any())
                return [];

            var mappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);

            foreach(var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
            }

            return mappedSessions;
        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetSessionWithTrainersAndCategory(sessionId);

            if (session == null)
                return null;

            var mappedSessions = _mapper.Map<Session, SessionViewModel>(session);

            
            mappedSessions.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
            

            return mappedSessions;
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(sessionId);

            if (session is null)
                return null;

            return _mapper.Map<UpdateSessionViewModel>(session);
        }

        public bool UpdateSession(int sessionId, UpdateSessionViewModel input)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(sessionId);

            if (!IsSessionAvailableForUpdate(session))
                return false;

            if (!IsTrainerExist(input.TrainerId))
                return false;

            if(!IsValidDateRange(input.StartDate, input.EndDate))
                return false;

            _mapper.Map<Session>(input);
            session.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.GetRepository<Session>().Update(session);

            return _unitOfWork.SaveChanges() > 0;


        }

        public bool RemoveSession(int sessionId)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(sessionId);

            if (!IsSessionAvailableForRemove(session))
                return false;

            _unitOfWork.GetRepository<Session>().Delete(session);

            return _unitOfWork.SaveChanges() > 0;

        }

        #region Helper Methods
        private bool IsTrainerExist(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);

            return trainer is null ? false : true;
        }

        private bool IsCategoryExist(int categoryId)
        {
            var category = _unitOfWork.GetRepository<Category>().GetById(categoryId);

            return category is null ? false : true;
        }

        private bool IsValidDateRange(DateTime startDate, DateTime endDate)
        {
            return endDate >= startDate && startDate > DateTime.UtcNow;
        }        

        private bool IsSessionAvailableForUpdate(Session session)
        {
            if (session is null)
                return false;

            if (session.EndDate < DateTime.UtcNow)
                return false;

            if (session.StartDate <= DateTime.UtcNow)
                return false;

            var hasActiveBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            if (hasActiveBookings)
                return false;

            return true;
        }

        private bool IsSessionAvailableForRemove(Session session)
        {
            if (session is null)
                return false;

            if (session.StartDate > DateTime.UtcNow)
                return false;

            if (session.StartDate <= DateTime.UtcNow && session.EndDate > DateTime.UtcNow)
                return false;

            var hasActiveBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            if (hasActiveBookings)
                return false;

            return true;
        }
        #endregion
    }
}
