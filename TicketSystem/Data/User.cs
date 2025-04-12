using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TicketSystem.Data
{
    public class User : IdentityUser<int>
    {
        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(500)]
        public string? Avatar { get; set; }
        [StringLength(12)]
        public string? NationalID { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentID { get; set; }
        public Department? Department { get; set; }

        [StringLength(10)]
        [Column(TypeName = "NVARCHAR(10)")]
        public string Status { get; set; } = "Active";

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsOnline { get; set; } = false;
        public DateTime? LastOnline { get; set; }
    }
}
