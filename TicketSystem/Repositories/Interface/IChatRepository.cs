using TicketSystem.Data;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories.Interface
{
    public interface IChatRepository
    {
        Task SendMessageAsync(int senderId, int receiverId, string content);
        Task<List<Message>> GetMessagesAsync(int userId, int otherUserId);
        Task<List<ChatUserVM>> GetChatUsersWithLastMessageAsync(int userId);
    }
}
