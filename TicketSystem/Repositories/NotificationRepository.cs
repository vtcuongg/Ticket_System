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

        public async Task<List<NotificationVM>> GetByUserId(int Userid)
        {
            var notifications = await _context.Notifications
            .Where(t => t.ReceiverID == Userid) 
            .ToListAsync();
            return notifications.Any()
          ? _mapper.Map<List<NotificationVM>>(notifications)
          : new List<NotificationVM>(); // Trả về danh sách rỗng thay vì null

        }
    }
}
