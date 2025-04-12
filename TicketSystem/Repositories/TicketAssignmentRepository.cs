using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Repositories.Interface;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories
{
    public class TicketAssignmentRepository : ITicketAssignmentRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public TicketAssignmentRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task Add(TicketAssignmentVM entity)
        {
            
            bool isExist = await _context.TicketAssignments
                                 .AnyAsync(ta => ta.TicketID == entity.TicketID && ta.AssignedTo == entity.AssignedTo);

            if (!isExist) // Nếu chưa tồn tại, thì thêm mới
            {
                var ticketAssignment = _mapper.Map<TicketAssignment>(entity);
                await _context.AddAsync(ticketAssignment);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Ticket này đã được dao cho nhân viên này rồi ");
            }
        }

        public async Task AssignUsersToTicket(string ticketId, List<int> assignedToList)
        {
            var existingAssignments = _context.TicketAssignments.Where(t => t.TicketID == ticketId);
            _context.TicketAssignments.RemoveRange(existingAssignments);
            // Thêm mới danh sách AssignedTo
            var newAssignments = assignedToList.Select(userId => new TicketAssignment
            {
                TicketID = ticketId,
                AssignedTo = userId
            }).ToList();

            await _context.TicketAssignments.AddRangeAsync(newAssignments);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var ticketAssignment = await _context.TicketAssignments.FindAsync(id);
            if (ticketAssignment != null)
            {
                _context.TicketAssignments.Remove(ticketAssignment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TicketAssignmentVM>> GetAll()
        {
            var ticketAssignments = await _context.TicketAssignments.ToListAsync();
            return _mapper.Map<IEnumerable<TicketAssignmentVM>>(ticketAssignments);
        }

        public async Task<TicketAssignmentVM?> GetById(int id)
        {
            var ticketAssignment = await _context.TicketAssignments.FindAsync(id);
            return ticketAssignment != null ? _mapper.Map<TicketAssignmentVM>(ticketAssignment) : null;
        }

        public async Task Update(TicketAssignmentVM entity)
        {
            var existingticketAssignment = await _context.TicketAssignments.FindAsync(entity.AssignmentID);
            if (existingticketAssignment != null)
            {
                _mapper.Map(entity, existingticketAssignment);
                _context.TicketAssignments.Update(existingticketAssignment);
                await _context.SaveChangesAsync();

            }
            else
            {
                throw new KeyNotFoundException($"Không tìm thấy User với ID = {entity.AssignmentID}");
            }
        }


    }
}
