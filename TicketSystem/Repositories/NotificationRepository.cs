using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Repositories.Interface;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories
{
    public class NotificationRepository : INotificationRepository

    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public NotificationRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task Add(NotificationVM entity)
        {
            var notification = _mapper.Map<Notification>(entity);
            await _context.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<NotificationVM_1>> GetByUserId(int Userid)
        {

            var notifications = await _context.Notifications
        .Where(t => t.ReceiverID == Userid)
        .Join(
            _context.Users, 
            notification => notification.SenderID,
            user => user.Id, 
            (notification, user) => new NotificationVM_1
            {
                NotificationID = notification.NotificationID,
                SenderID = notification.SenderID,
                ReceiverID = notification.ReceiverID,
                Message = notification.Message,
                TicketID = notification.TicketID,
                CreatedAt = notification.CreatedAt,
                IsRead = notification.IsRead,
                SenderName = user.UserName,
                SenderAvatar = user.Avatar
            }
        )
        .ToListAsync();

            return notifications.Any()
                ? notifications 
                : new List<NotificationVM_1>();

        }

    }
}
