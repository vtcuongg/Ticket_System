using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TicketSystem.Data;

namespace TicketSystem.ViewModel
{
    public class TicketVM
    {
       
        public string? TicketID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = "Thấp";
        public string Status { get; set; } = "Mới";
        public int CreatedBy { get; set; }
        public int? DepartmentID { get; set; }
        public int? CategoryID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public Boolean? IsFeedBack { get; set; }
        public List<IFormFile>? Attachments { get; set; }

    }
}
