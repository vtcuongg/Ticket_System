using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Data
{
    public class TicketAttachment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FileUrl { get; set; } = string.Empty;

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Ticket")]
        public string TicketID { get; set; }

        public Ticket? Ticket { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
