using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Repositories.Interface;
using TicketSystem.Service;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories
{
    public class ChatHub :Hub
    {
        private readonly MyDbContext _context;
        private readonly IChatRepository _chatRepository;
        public ChatHub(IChatRepository chatRepository, MyDbContext context)
        {
            _chatRepository = chatRepository;
            _context = context;
        }
        public override async Task OnConnectedAsync()
        {
            var userId = GetUserId();
            if (userId == 0)
            {
                await base.OnConnectedAsync();
                return;
            }

            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsOnline = true;
                user.LastOnline = null;
                await _context.SaveChangesAsync();
                await Clients.Others.SendAsync("UserOnline", userId); 
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = GetUserId();
            if (userId == 0)
            {
                await base.OnDisconnectedAsync(exception);
                return;
            }

            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsOnline = false;
                user.LastOnline = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                await Clients.Others.SendAsync("UserOffline", userId);
            }

            await base.OnDisconnectedAsync(exception);
        }
        private int GetUserId()
        {
            var userId = Context.UserIdentifier;
            return int.TryParse(userId, out var id) ? id : 0;
           
        }
        public async Task MarkAsRead(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message != null && !message.IsRead)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
                var senderId = message.SenderID?.ToString()
               ?? throw new InvalidOperationException("SenderID is null");
                await Clients.User(senderId).SendAsync("MessageRead", message.MessageID);
            }
        }
    }
}
