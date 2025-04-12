using TicketSystem.ViewModel;

namespace TicketSystem.Models
{
    public class SearchTicketResult
    {
        public string? TicketID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = "Thấp";
        public string Status { get; set; } = "Mới";
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public string? CreatedByAvatar { get; set; }
        public int DepartmentID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? IsFeedBack { get; set; }

        // Thông tin người xử lý
        public int? AssignmentID { get; set; }
        public int? AssignedTo { get; set; }
        public string? UserName { get; set; }
        public string? Avatar { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set;}
    }

}
