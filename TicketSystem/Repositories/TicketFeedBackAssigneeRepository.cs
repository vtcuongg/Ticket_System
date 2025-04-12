using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Repositories.Interface;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories
{
    public class TicketFeedBackAssigneeRepository: ITicketFeedBackAssigneeRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public TicketFeedBackAssigneeRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task Add(TicketFeedBackAssigneeVM entity)
        {
            var ticketFeedbackAssignee = _mapper.Map<TicketFeedbackAssignee>(entity);
            await _context.AddAsync(ticketFeedbackAssignee);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string TicketId, int assignedTo)
        {
            var entity = _context.TicketFeedbackAssignees
         .FirstOrDefault(tfa => tfa.TicketID == TicketId && tfa.AssignedTo == assignedTo);
            if (entity != null)
            {
                _context.TicketFeedbackAssignees.Remove(entity);
                await _context.SaveChangesAsync();
            }

        }




    }
}
