using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool Activate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);

            if (plan is null || HasActiveMemberships(planId))
                    return false;

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.GetRepository<Plan>().Update(plan);

            return _unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();

            if (plans is null || !plans.Any())
                return [];

            return plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description, 
                DurationDays = p.DurationDays,
                Price = p.Price,
                IsActive = p.IsActive
            });

        }

        public PlanViewModel GetPlanById(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);

            if (plan is null)
                return null;

            return  new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            };
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);

            if (plan is null || plan.IsActive == false)
                return null;

            return new UpdatePlanViewModel
            {                
                PlanName = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price
            };
        }

        public bool UpdatePlan(int id, UpdatePlanViewModel input)
        {
            try
            {
                var plan = _unitOfWork.GetRepository<Plan>().GetById(id);

                if(plan is null || HasActiveMemberships(id))
                    return false;

                plan.Description = input.Description;
                plan.Price = input.Price; 
                plan.DurationDays = input.DurationDays;
                plan.Name = input.PlanName;

                _unitOfWork.GetRepository<Plan>().Update(plan);
                
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Helper Methods
        private bool HasActiveMemberships(int planId)
        {
            return _unitOfWork.GetRepository<Membership>()
                .GetAll(m => m.PlanId == planId && m.Status == "Active")
                .Any();
        }
        #endregion
    }
}
