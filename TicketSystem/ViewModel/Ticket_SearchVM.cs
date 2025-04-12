namespace TicketSystem.ViewModel
{
    public class Ticket_SearchVM
    {
        public string? TicketID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = "Thấp";
        public string Status { get; set; } = "Mới";
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public string? CreatedByAvatar { get; set; }
        public int? DepartmentID { get; set; }
        public int? CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public Boolean? IsFeedBack { get; set; }
        public List<AssignmentVM> AssignedUsers { get; set; } = new();
        public List<AttachmentVM> Attachments { get; set; } = new();
    }
}
