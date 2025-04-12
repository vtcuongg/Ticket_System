using TicketSystem.ViewModel;

namespace TicketSystem.Repositories.Interface
{
    public interface ITicketFeedBackRepository
    {
        Task<IEnumerable<TicketFeedBackVM>> GetAll();
        Task<TicketFeedBackVM?> GetById(int id);
        Task<TicketFeedBackVM?> GetByTicketId(string id);
        Task Add(TicketFeedBackVM entity);
        Task Update(TicketFeedBackVM entity);
        Task Delete(int id);
    }
}
