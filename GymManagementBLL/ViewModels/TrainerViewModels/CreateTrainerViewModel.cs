using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.TrainerViewModels
{
    public class CreateTrainerViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name must contain only letters and single spaces between words.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}", ErrorMessage = "Must be a valid Egyptian phone number.")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }
        [Required(ErrorMessage = "Building number is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Building Number must be greater than 0.")]
        public int BuildingNumber { get; set; }
        [Required(ErrorMessage = "City is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters.")]
        [RegularExpression(@"^[a-zA-z\s]+$", ErrorMessage = "City can only contain letters and spaces.")]
        public string City { get; set; } = null!;
        [Required(ErrorMessage = "Street is required.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "City must be between 2 and 150 characters.")]
        [RegularExpression(@"^[a-zA-z0-9\s]+$", ErrorMessage = "City can only contain letters, numbers and spaces.")]
        public string Street { get; set; } = null!;
        [Required(ErrorMessage = "Health record is required.")]
        public HealthRecordViewModel HealthRecordViewModel { get; set; } = null!;
    }
}
