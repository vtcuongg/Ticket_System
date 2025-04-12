using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Data
{
    public class Department
    {
        [Key]

        public int DepartmentID { get; set; }

        [Required, StringLength(255)]
        public string DepartmentName { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
