using TicketSystem.ViewModel;

namespace TicketSystem.Repositories.Interface
{
    public interface ITicketAssignmentRepository
    {
        Task<IEnumerable<TicketAssignmentVM>> GetAll();
        Task<TicketAssignmentVM?> GetById(int id);
        Task Add(TicketAssignmentVM entity);
        Task AssignUsersToTicket(string ticketId, List<int> assignedToList);
        Task Update(TicketAssignmentVM entity);
        Task Delete(int id);
    }
}
