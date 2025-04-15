using TicketSystem.Data;
using TicketSystem.Service;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories.Interface
{
    public interface IChatRepository
    {
        Task SendMessageAsync(SendMessageRequestVM entity , IS3Service s3Service);
        Task<List<MessageVM>?> GetMessagesAsync(int userId, int otherUserId);
        Task<List<ChatUserVM>?> GetChatUsersWithLastMessageAsync(int userId);
    }
}
