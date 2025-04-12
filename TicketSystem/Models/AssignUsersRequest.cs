namespace TicketSystem.Models
{
    public class AssignUsersRequest
    {
        public string? TicketID { get; set; }
        public List<int> AssignedToList { get; set; } = new();
    }
}
