using System.ComponentModel.DataAnnotations;

namespace TicketSystem.ViewModel
{
    public class TicketFeedBackAssigneeVM
    {
        public string? TicketID { get; set; }
        public int AssignedTo { get; set; }
    }
}
