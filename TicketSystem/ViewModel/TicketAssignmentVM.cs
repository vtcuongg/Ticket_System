using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TicketSystem.Data;

namespace TicketSystem.ViewModel
{
    public class TicketAssignmentVM
    {
        public int AssignmentID { get; set; }
        public string? TicketID { get; set; }
        public int AssignedTo { get; set; }


    }
}
