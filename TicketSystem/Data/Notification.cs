using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Data
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        public int? SenderID { get; set; }
        [ForeignKey("SenderID")]
        public User? Sender { get; set; }

        public int? ReceiverID { get; set; } 
        [ForeignKey("ReceiverID")]
        public User? Receiver { get; set; }

        [StringLength(50)]
        public string? TicketID { get; set; } 
      

        [Required]
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false; 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    }
}
