using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Data
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        public int? SenderID { get; set; } // Người gửi thông báo (có thể là hệ thống)
        [ForeignKey("SenderID")]
        public User? Sender { get; set; }

        public int? ReceiverID { get; set; } // Người nhận thông báo
        [ForeignKey("ReceiverID")]
        public User? Receiver { get; set; }

        [StringLength(50)]
        public string? TicketID { get; set; } // Liên kết với Ticket nếu có
        [ForeignKey("TicketID")]
        public Ticket? Ticket { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty; // Nội dung thông báo

        public bool IsRead { get; set; } = false; // Trạng thái đã đọc

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Thời gian tạo
    }
}
