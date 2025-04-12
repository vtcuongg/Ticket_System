using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Repositories.Interface;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories
{
    public class TicketFeedBackRepository : ITicketFeedBackRepository

    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public TicketFeedBackRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task Add(TicketFeedBackVM entity)
        {
            var ticketFeedBack = _mapper.Map<TicketFeedBack>(entity);
            await _context.AddAsync(ticketFeedBack);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var ticketFeedBack = await _context.TicketFeedBacks.FindAsync(id);
            if (ticketFeedBack != null)
            {
                _context.TicketFeedBacks.Remove(ticketFeedBack);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TicketFeedBackVM>> GetAll()
        {
            var ticketFeedBack = await _context.TicketFeedBacks.ToListAsync();
            return _mapper.Map<IEnumerable<TicketFeedBackVM>>(ticketFeedBack);
        }

        public async Task<TicketFeedBackVM?> GetById(int id)
        {
            var ticketFeedBack = await _context.TicketFeedBacks.FindAsync(id);
            return ticketFeedBack != null ? _mapper.Map<TicketFeedBackVM>(ticketFeedBack) : null;
        }

        public async Task<TicketFeedBackVM?> GetByTicketId(string id)
        {
            var ticketFeedBack = await _context.TicketFeedBacks
                                       .FirstOrDefaultAsync(t => t.TicketID == id);
            return ticketFeedBack != null ? _mapper.Map<TicketFeedBackVM>(ticketFeedBack) : null;

        }

        public async Task Update(TicketFeedBackVM entity)
        {
            var existingTicketFeedBack = await _context.TicketFeedBacks.FindAsync(entity.FeedbackID);
            if (existingTicketFeedBack != null)
            {
                _mapper.Map(entity, existingTicketFeedBack);
                _context.TicketFeedBacks.Update(existingTicketFeedBack);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Không tìm thấy Feedback với ID = {entity.FeedbackID}");
            }
        }
    }
}
