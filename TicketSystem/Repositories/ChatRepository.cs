using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net.Sockets;
using TicketSystem.Data;
using TicketSystem.Repositories.Interface;
using TicketSystem.Service;
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
        public async Task SendMessageAsync(SendMessageRequestVM entity , IS3Service  s3Service  )
        {
            var message = new Message
            {
                SenderID = entity.SenderId,
                ReceiverID = entity.ReceiverId,
                Content = entity.Content,
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };
            if (entity.Attachments != null && entity.Attachments.Any())
            {
                message.Attachments = new List<MessageAttachment>();

                foreach (var file in entity.Attachments)
                {
                    var fileName = $"MessageaAttachments/{message.SenderID}/{Guid.NewGuid()}_{file.FileName}";
                    var fileUrl = await s3Service.UploadFileAsync(file, fileName);

                    message.Attachments.Add(new MessageAttachment
                    {
                        FileName = file.FileName,
                        FileUrl = fileUrl,
                        UploadedAt = DateTime.UtcNow
                    });
                }
            }
            await _context.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MessageVM>?> GetMessagesAsync(int userId, int otherUserId)
        {
             var messages= await _context.Messages
                .Where(m => (m.SenderID == userId && m.ReceiverID == otherUserId) ||
                            (m.SenderID == otherUserId && m.ReceiverID == userId))
                .Include(m => m.Attachments) 
                .OrderByDescending(m => m.Timestamp)
                .ToListAsync();
            if (messages == null)
            {
                return new List<MessageVM>();
            }
            var result = messages.Select(m => new MessageVM
            {
                MessageID = m.MessageID,
                SenderID = m.SenderID,
                ReceiverID = m.ReceiverID,
                Content = m.Content,
                Timestamp = m.Timestamp,
                IsRead = m.IsRead,
                Attachments = m.Attachments != null
                ? m.Attachments.Select(a => new AttachmentVM
                {
                    FileName = a.FileName,
                    Url = a.FileUrl,
                }).ToList()
                : new List<AttachmentVM>()

            }).ToList();

            return result;
        }
        public async Task<List<ChatUserVM>?> GetChatUsersWithLastMessageAsync(int userId)
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
            var lastMessageIds = lastMessages.Select(m => m!.MessageID).ToList();
            var attachments = await _context.MessageAttachments
                .Where(a => lastMessageIds.Contains(a.MessageId))
                .ToListAsync();

            var userIds = lastMessages.Select(m => m?.PartnerID).ToList() ;
            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            var userChatList = users.Select(u =>
            {
                var lastMessage = lastMessages.FirstOrDefault(m => m?.PartnerID == u.Id);
                var messageAttachments = attachments
                    .Where(a => a.MessageId == lastMessage?.MessageID)
                    .Select(a => new AttachmentVM
                    {
                        FileName = a.FileName,
                        Url = a.FileUrl
                    }).ToList();
                return new ChatUserVM
                {
                    UserID = u.Id,
                    FullName = u.UserName ?? string.Empty,
                    Email = u.Email ?? string.Empty,
                    Avatar = u.Avatar ?? string.Empty,
                    LastMessageId= lastMessage?.MessageID  ,
                    IsRead = lastMessage != null ? lastMessage.IsRead : false,
                    LastMessageContent = lastMessage?.Content,
                    LastMessageTime = lastMessage?.Timestamp ?? DateTime.MinValue,
                    LastMessageSenderID = lastMessage?.SenderID ?? 0,
                    IsOnline = u.IsOnline,
                    LastOnline = u.LastOnline,
                    Attachments = messageAttachments
                };
            })
            .OrderByDescending(u => u.LastMessageTime)
            .ToList();

            return userChatList;
        }
    }
}
