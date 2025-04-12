using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Repositories.Interface;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly MyDbContext _context;

        public ChatRepository(MyDbContext context)
        {
            _context = context;
        }
        public async Task SendMessageAsync(int senderId, int receiverId, string content)
        {
            var message = new Message
            {
                SenderID = senderId,
                ReceiverID = receiverId,
                Content = content,
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Message>> GetMessagesAsync(int userId, int otherUserId)
        {
            return await _context.Messages
                .Where(m => (m.SenderID == userId && m.ReceiverID == otherUserId) ||
                            (m.SenderID == otherUserId && m.ReceiverID == userId))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }
        public async Task<List<ChatUserVM>> GetChatUsersWithLastMessageAsync(int userId)
        {
            var lastMessages = await _context.Messages
                .Where(m => m.SenderID == userId || m.ReceiverID == userId)
                .Select(m => new
                {
                    m.MessageID,
                    m.SenderID,
                    m.ReceiverID,
                    m.Content,
                    m.Timestamp,
                    m.IsRead,
                    PartnerID = m.SenderID == userId ? m.ReceiverID : m.SenderID
                })
                .GroupBy(m => m.PartnerID)
                .Select(g => g.OrderByDescending(m => m.Timestamp).FirstOrDefault())
                .ToListAsync();

            var userIds = lastMessages.Select(m => m.PartnerID).ToList();
            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            var userChatList = users.Select(u =>
            {
                var lastMessage = lastMessages.FirstOrDefault(m => m.PartnerID == u.Id);
                return new ChatUserVM
                {
                    UserID = u.Id,
                    FullName = u.UserName,
                    Email = u.Email,
                    Avatar = u.Avatar,
                    LastMessageId= lastMessage.MessageID,
                    IsRead= lastMessage.IsRead,
                    LastMessageContent = lastMessage?.Content,
                    LastMessageTime = lastMessage?.Timestamp ?? DateTime.MinValue,
                    LastMessageSenderID = lastMessage?.SenderID ?? 0,
                    IsOnline = u.IsOnline,
                    LastOnline = u.LastOnline
                };
            })
            .OrderByDescending(u => u.LastMessageTime)
            .ToList();

            return userChatList;
        }
    }
}
