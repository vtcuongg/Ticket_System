using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Data
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required, StringLength(255)]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        public int DepartmentID { get; set; }

        [ForeignKey("DepartmentID")]
        public Department? Department { get; set; }
    }
}
