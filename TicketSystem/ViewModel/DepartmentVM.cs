using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TicketSystem.ViewModel
{
    public class DepartmentVM
    {
     
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; } = string.Empty;

    }
}
