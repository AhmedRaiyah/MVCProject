using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AnalyticalViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class AnalyticalService : IAnalyticalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public AnalyticalViewModel GetAnalyticsData()
        {
            var sessionRepository = _unitOfWork.GetRepository<Session>();
            return new AnalyticalViewModel                
            {
                ActiveMembers = _unitOfWork.GetRepository<Membership>().GetAll(x => x.Status == "Active").Count(),
                TotalMembers = _unitOfWork.GetRepository<Member>().GetAll().Count(),
                TotalTrainers = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                UpcomingSessions = sessionRepository.GetAll(x => x.StartDate > DateTime.UtcNow).Count(),
                OngoingSessions = sessionRepository.GetAll(x => x.StartDate <= DateTime.UtcNow && x.EndDate >= DateTime.UtcNow).Count(),
                CompletedSessions = sessionRepository.GetAll(x => x.EndDate < DateTime.UtcNow).Count(),

            };
        }
    }
}
