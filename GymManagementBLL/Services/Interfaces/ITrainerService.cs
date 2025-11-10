using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        bool CreateTrainer(CreateTrainerViewModel model);
        bool UpdateTrainerDetails(int memberId, TrainerToUpdateViewModel model);
        bool RemoveTrainer(int trainerId);
        IEnumerable<TrainerViewModel> GetAllTrainers();
        TrainerViewModel? GetTrainerDetails(int trainerId);
        TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId);
    }
}
