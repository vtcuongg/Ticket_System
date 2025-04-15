using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Data
{
    public class MessageAttachment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FileUrl { get; set; } = string.Empty;

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Message")]
        public int? MessageId { get; set; }

        public Message? Message { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
