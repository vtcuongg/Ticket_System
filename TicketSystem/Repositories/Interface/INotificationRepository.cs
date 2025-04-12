using TicketSystem.ViewModel;

namespace TicketSystem.Repositories.Interface
{
    public interface INotificationRepository
    {
        Task<List<NotificationVM>> GetByUserId(int id);
        Task Add(NotificationVM entity);
    }
}
