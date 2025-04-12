using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TicketSystem.Data;

namespace TicketSystem.ViewModel
{
    public class TicketFeedBackVM
    {
        public int FeedbackID { get; set; }
        public string? TicketID { get; set; }
        public int? CreatedBy { get; set; } // Người tạo feedback
        public int Rating { get; set; }
        public string? Comment { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
