using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Data
{
    public class TicketAssignment
    {
        [Key]
        public int AssignmentID { get; set; }

        [ForeignKey("Ticket")]
        public string? TicketID { get; set; }
        public Ticket Ticket { get; set; } = null!;

        [ForeignKey("User")]
        public int AssignedTo { get; set; }
        public User User { get; set; } = null!;
    }
}
