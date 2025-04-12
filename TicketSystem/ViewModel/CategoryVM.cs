using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TicketSystem.Data;
using System.Text.Json.Serialization;

namespace TicketSystem.ViewModel
{
    public class CategoryVM
    {
     
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int DepartmentID { get; set; }

    }
}
