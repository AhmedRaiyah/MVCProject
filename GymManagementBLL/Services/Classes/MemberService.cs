using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateMember(CreateMemberViewModel model)
        {
            try
            {
                if (IsEmailExists(model.Email))
                    return false;
                if (IsPhoneExists(model.Phone))
                    return false;

                var member = new Member
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
                    },
                    HealthRecord = new HealthRecord
                    {
                        Height = model.HealthRecordViewModel.Height,
                        Weight = model.HealthRecordViewModel.Weight,
                        BloodType = model.HealthRecordViewModel.BloodType,
                        Note = model.HealthRecordViewModel.Note

                    }
                };

                _unitOfWork.GetRepository<Member>().Add(member);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll() ?? [];

            if (members is null || !members.Any())
                return [];

            var memberViewModels = members.Select(x => new MemberViewModel
            {
                Id = x.Id,
                Photo = x.Photo,
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                DateOfBirth = x.DateOfBirth.ToShortDateString(),
                Gender = x.Gender.ToString()                
            });

            return memberViewModels;
        }

        public MemberViewModel? GetMemberDetails(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);

            if (member is null)
                return null;

            var memberViewModel = new MemberViewModel
            {
                Id = member.Id,
                Photo = member.Photo,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address = FormatAddress(member.Address)
            };

            var activeMembership = _unitOfWork.GetRepository<Membership>()
                                  .GetAll(x => x.MemberId == memberId && x.Status == "Active")
                                  .FirstOrDefault();

            if(activeMembership is not null)
            {
                var activePlan = _unitOfWork.GetRepository<Plan>().GetById(activeMembership.PlanId);

                memberViewModel.PlanName = activePlan?.Name;
                memberViewModel.MembershipStartDate = activeMembership.CreatedAt.ToShortDateString();
                memberViewModel.MembershipEndDate = activeMembership.EndDate.ToShortDateString();
            }

            return memberViewModel;
        }

        public HealthRecordViewModel? GetMemberHealthRecord(int memberId)
        {
            var memberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(memberId);

            if (memberHealthRecord is null)
                return null;

            return new HealthRecordViewModel
            {
                Height = memberHealthRecord.Height,
                Weight = memberHealthRecord.Weight,
                BloodType = memberHealthRecord.BloodType,
                Note = memberHealthRecord.Note
            };
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);

            if (member is null)
                return null;

            var memberToUpdateViewModel = new MemberToUpdateViewModel
            {
                Photo = member.Photo,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                BuildingNumber = member.Address.BuildingNumber,
                City = member.Address.City,
                Street = member.Address.Street,
            };

            return memberToUpdateViewModel;
        }

        public bool RemoveMember(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);

            if (member is null)
                return false;

            var activeBookings = _unitOfWork.GetRepository<Booking>()
                                .GetAll(x => x.MemberId == memberId && x.Session.StartDate > DateTime.UtcNow);

            if (activeBookings.Any())
                return false;

            var memberships = _unitOfWork.GetRepository<Membership>().GetAll(x => x.MemberId == memberId).ToList();

            try
            {
                if(memberships.Any())
                {
                    foreach( var membership in memberships)
                    {
                        _unitOfWork.GetRepository<Membership>().Delete(membership);
                    }

                    _unitOfWork.GetRepository<Member>().Delete(member);
                }

                    return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UpdateMemberDetails(int memberId, MemberToUpdateViewModel model)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);

            if (member is null)
                return false;

            if (IsEmailExists(model.Email))
                return false;
            if (IsPhoneExists(model.Phone))
                return false;

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Member>().Update(member);

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
