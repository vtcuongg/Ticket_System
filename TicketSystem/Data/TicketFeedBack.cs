using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Data
{
    public class TicketFeedBack
    {
        [Key]
        public int FeedbackID { get; set; }

        [Required]
        public string? TicketID { get; set; }

        [Required]
        public int? CreatedBy { get; set; } 

        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("TicketID")]
        public virtual Ticket? Ticket { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User? User { get; set; }
    }
}
