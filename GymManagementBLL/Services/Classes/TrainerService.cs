using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateTrainer(CreateTrainerViewModel model)
        {
            try
            {
                if (IsEmailExists(model.Email))
                    return false;
                if (IsPhoneExists(model.Phone))
                    return false;

                var trainer = new Trainer
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    Address = new Address
                    {
                        BuildingNumber = model.BuildingNumber,
                        City = model.City,
                        Street = model.Street,
                    }
                };

                _unitOfWork.GetRepository<Trainer>().Add(trainer);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll() ?? [];

            if (trainers is null || !trainers.Any())
                return [];

            var trainerViewModels = trainers.Select(x => new TrainerViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                DateOfBirth = x.DateOfBirth.ToShortDateString(),
                Gender = x.Gender.ToString()
            });

            return trainerViewModels;
        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);

            if (trainer is null)
                return null;

            var trainerViewModel = new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                Gender = trainer.Gender.ToString(),
                Address = FormatAddress(trainer.Address)
            };


            return trainerViewModel;
        }

        

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);

            if (trainer is null)
                return null;

            var trainerToUpdateViewModel = new TrainerToUpdateViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                City = trainer.Address.City,
                Street = trainer.Address.Street,
            };

            return trainerToUpdateViewModel;
        }

        public bool RemoveTrainer(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);

            if (trainer is null)
                return false;


            _unitOfWork.GetRepository<Trainer>().Delete(trainer);
            return true;

            //var activeBookings = _unitOfWork.GetRepository<Booking>()
            //                    .GetAll(x => x.MemberId == trainerId && x.Session.StartDate > DateTime.UtcNow);

            //if (activeBookings.Any())
            //    return false;

            //var memberships = _unitOfWork.GetRepository<Membership>().GetAll(x => x.MemberId == memberId).ToList();

            //try
            //{
            //    if (memberships.Any())
            //    {
            //        foreach (var membership in memberships)
            //        {
            //            _unitOfWork.GetRepository<Membership>().Delete(membership);
            //        }

            //        _unitOfWork.GetRepository<Member>().Delete(member);
            //    }

            //    return true;
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }

        public bool UpdateTrainerDetails(int trainerId, TrainerToUpdateViewModel model)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);

            if (trainer is null)
                return false;

            if (IsEmailExists(model.Email))
                return false;
            if (IsPhoneExists(model.Phone))
                return false;

            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Address.City = model.City;
            trainer.Address.Street = model.Street;
            trainer.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Trainer>().Update(trainer);

            return true;
        }


        #region Helper Methods
        private string FormatAddress(Address address)
        {
            if (address is null)
                return "N/A";

            return $"{address.BuildingNumber}, {address.Street}, {address.City}";
        }

        private bool IsEmailExists(string email)
        {
            var existingMember = _unitOfWork.GetRepository<Member>().GetAll(x => x.Email == email);
            return existingMember is not null && existingMember.Any();
        }

        private bool IsPhoneExists(string phone)
        {
            var existingMember = _unitOfWork.GetRepository<Member>().GetAll(x => x.Phone == phone);
            return existingMember is not null && existingMember.Any();
        }
        #endregion
    }
}
