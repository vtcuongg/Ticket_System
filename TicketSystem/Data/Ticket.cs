using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Data
{
    public class Ticket
    {
        [Key]
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Không tự động tăng
        [Required, StringLength(50)]
        public string? TicketID { get; set; }

        [Required, StringLength(255)]
         public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Priority { get; set; } = "Thấp";

        [Required, StringLength(20)]
        public string Status { get; set; } = "Mới";

        [Required]
        public int CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public User? User { get; set; }

        public int? DepartmentID { get; set; }
        [ForeignKey("DepartmentID")]
        public Department? Department { get; set; }

        public int? CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category? Category { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DueDate { get; set; }
        public Boolean? IsFeedBack { get; set; }
        public ICollection<TicketAttachment>? Attachments { get; set; }
    }
}
