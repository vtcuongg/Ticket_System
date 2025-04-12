using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TicketSystem.Data;

namespace TicketSystem.ViewModel
{
    public class NotificationVM
    {
        public int NotificationID { get; set; }

        public int? SenderID { get; set; } 
        public int? ReceiverID { get; set; } 
        public string? TicketID { get; set; } 
        public string Message { get; set; } = string.Empty; 
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
