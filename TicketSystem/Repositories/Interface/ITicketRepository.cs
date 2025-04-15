using TicketSystem.Service;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories.Interface
{
    public interface ITicketRepository
    {
        
        Task<TicketVM?> GetById(string id);
        Task<IEnumerable<TicketVM>?> GetByDepartmentId(int id);
        Task Add(TicketVM entity,IS3Service s3Service);
        Task Update(TicketVM entity, IS3Service s3Service);
        Task Delete(string id);
        Task UpdateStatus(string TicketId, string newStatus);
        Task UpdatePriority(string TicketId, string newPriority);
        Task UpdateIsFeedBack(string TicketId, Boolean newIsFeedBack);
        Task<IEnumerable<Ticket_SearchVM>> SearchTickets(string? TicketID,string? title, int? day, int? month, int? year,
    int? createdBy, int? departmentId, int? assignedTo);

    }
}
