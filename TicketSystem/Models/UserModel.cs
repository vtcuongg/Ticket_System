using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Models
{
    public class UserModel
    {
        public string UserName { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
      
        [RegularExpression("^0[0-9]{9}$", ErrorMessage = "Phone number must be 10 digits and start with 0.")]
        public string PhoneNumber { get; set; }=string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        [Required]
        [RegularExpression("^[0-9]{12}$", ErrorMessage = "National ID must be exactly 12 digits.")]
        public string? NationalID { get; set; } 
        public int? DepartmentID { get; set; }
        public int? RoleID { get; set; }
        [Required]
        [RegularExpression("^(?=.*[A-Z])(?=.*[!@#$%^&*]).{6,}$", ErrorMessage = "Password must be at least 6 characters long, contain at least one uppercase letter, and one special character.")]
        public string? PasswordHash { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";

        public DateTime CreatedAt { get; set; }
    }
}
