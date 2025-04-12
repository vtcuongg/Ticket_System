using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Data
{
    public class TicketFeedbackAssignee
    {
        [Key]
        public int TicketFeedbackAssigneeID { get; set; }

        [Required] 
        [StringLength(50)]
        public string? TicketID { get; set; }

        [Required]  
        public int AssignedTo { get; set; }
       
        [ForeignKey(nameof(TicketID))] 
        public virtual Ticket? Ticket { get; set; }

        [ForeignKey(nameof(AssignedTo))]
        public virtual User? User { get; set; }
    }
}
