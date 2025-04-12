using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Repositories.Interface;
using TicketSystem.Service;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories
{
    public class TicketRepository : ITicketRepository
    {

        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public TicketRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<string> GenerateTicketCode()
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "sp_GenerateTicketCode";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                await _context.Database.OpenConnectionAsync();
                var result = await command.ExecuteScalarAsync();
                await _context.Database.CloseConnectionAsync();

                return result?.ToString() ?? throw new Exception("Không thể tạo TicketID.");
            }
        }
        public async Task Add(TicketVM entity, IS3Service s3Service)
        {
            var ticket = _mapper.Map<Ticket>(entity);
            ticket.TicketID = await GenerateTicketCode();
            // Xử lý file đính kèm nếu có
            if (entity.Attachments != null && entity.Attachments.Any())
            {
                ticket.Attachments = new List<TicketAttachment>();

                foreach (var file in entity.Attachments)
                {
                    var fileName = $"attachments/{ticket.TicketID}/{Guid.NewGuid()}_{file.FileName}";
                    var fileUrl = await s3Service.UploadFileAsync(file, fileName);

                    ticket.Attachments.Add(new TicketAttachment
                    {
                        FileName = file.FileName,
                        FileUrl = fileUrl,
                        UploadedAt = DateTime.UtcNow
                    });
                }
            }
            await _context.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        //public async Task<IEnumerable<TicketVM>> GetAll()
        //{
        //    var tickets = await _context.Tickets.ToListAsync();
        //    return _mapper.Map<IEnumerable<TicketVM>>(tickets);
        //}

        public async Task<TicketVM?> GetById(string id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            return ticket != null ? _mapper.Map<TicketVM>(ticket) : null;
        }

        //public async Task<IEnumerable<TicketVM>?> GetByUserId(int UserId)
        //{
        //    var tickets = await _context.Tickets
        //                        .Where(t => t.CreatedBy == UserId)
        //                        .ToListAsync();
        //    return tickets != null ? _mapper.Map<List<TicketVM>>(tickets) : null;
        //}

        public async Task Update(TicketVM entity)
        {
            var existingTicket = await _context.Tickets.FindAsync(entity.TicketID);
            if (existingTicket != null)
            {
                _mapper.Map(entity, existingTicket);
                _context.Tickets.Update(existingTicket);
                await _context.SaveChangesAsync();

            }
            else
            {
                throw new KeyNotFoundException($"Không tìm thấy User với ID = {entity.TicketID}");
            }
        }
        public async Task UpdateStatus(string ticketId, string newStatus)
        {
            var existingTicket = await _context.Tickets.FindAsync(ticketId);
            if (existingTicket != null)
            {
                existingTicket.Status = newStatus;
                existingTicket.UpdatedAt = DateTime.UtcNow; // Cập nhật thời gian sửa đổi

                _context.Tickets.Update(existingTicket);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Không tìm thấy Ticket với ID = {ticketId}");
            }
        }
        public async Task UpdatePriority(string ticketId, string newPriority)
        {
            var existingTicket = await _context.Tickets.FindAsync(ticketId);
            if (existingTicket != null)
            {
                existingTicket.Priority = newPriority;
                existingTicket.UpdatedAt = DateTime.UtcNow; // Cập nhật thời gian sửa đổi

                _context.Tickets.Update(existingTicket);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Không tìm thấy Ticket với ID = {ticketId}");
            }
        }
        public async Task UpdateIsFeedBack(string ticketId, Boolean newIsFeedBack)
        {
            var existingTicket = await _context.Tickets.FindAsync(ticketId);
            if (existingTicket != null)
            {
                existingTicket.IsFeedBack = newIsFeedBack;
                existingTicket.UpdatedAt = DateTime.UtcNow; // Cập nhật thời gian sửa đổi

                _context.Tickets.Update(existingTicket);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Không tìm thấy Ticket với ID = {ticketId}");
            }
        }

        public async Task<IEnumerable<Ticket_SearchVM>> SearchTickets(
        string? ticketId, string? title, int? day, int? month, int? year,
        int? createdBy, int? departmentId, int? assignedTo)
        {
            var result = await _context.SearchTicketResults
       .FromSqlRaw("EXEC sp_SearchTickets @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7",
         ticketId ?? (object)DBNull.Value,
           title ?? (object)DBNull.Value,
           day ?? (object)DBNull.Value,
           month ?? (object)DBNull.Value,
           year ?? (object)DBNull.Value,
           createdBy ?? (object)DBNull.Value,
           departmentId ?? (object)DBNull.Value,
           assignedTo ?? (object)DBNull.Value)
       .ToListAsync();

            var tickets = result
                .GroupBy(t => new
                {
                    t.TicketID,
                    t.Title,
                    t.Description,
                    t.Priority,
                    t.Status,
                    t.CreatedBy,
                    t.CreatedByName,
                    t.CreatedByAvatar,
                    t.DepartmentID,
                    t.CategoryID,
                    t.CategoryName,
                    t.CreatedAt,
                    t.UpdatedAt,
                    t.DueDate,
                    t.IsFeedBack
                })
                .Select(group => new Ticket_SearchVM
                {
                    TicketID = group.Key.TicketID,
                    Title = group.Key.Title,
                    Description = group.Key.Description,
                    Priority = group.Key.Priority,
                    Status = group.Key.Status,
                    CreatedBy = group.Key.CreatedBy,
                    CreatedByName = group.Key.CreatedByName,
                    CreatedByAvatar = group.Key.CreatedByAvatar,
                    DepartmentID = group.Key.DepartmentID,
                    CategoryID = group.Key.CategoryID,
                    CategoryName = group.Key.CategoryName,
                    CreatedAt = group.Key.CreatedAt,
                    UpdatedAt = group.Key.UpdatedAt,
                    DueDate = group.Key.DueDate,
                    IsFeedBack = group.Key.IsFeedBack,
                    AssignedUsers = group
                        .Where(g => g.AssignmentID != null)
                        .Select(g => new AssignmentVM
                        {
                            AssignmentID = g.AssignmentID,
                            AssignedTo = g.AssignedTo,
                            UserName = g.UserName,
                            Avatar = g.Avatar
                        }).ToList(),
                    Attachments = group
                        .Where(x => x.FileName != null && x.FileUrl != null)
                        .Select(g => new AttachmentVM
                        {
                            FileName = g.FileName,
                            Url = g.FileUrl
                        }).ToList(),

                }).ToList();
            return tickets;

        }
        public async Task<IEnumerable<TicketVM>?> GetByDepartmentId(int departmentId)
        {
            var tickets = await _context.Tickets
                                .Where(t => t.DepartmentID == departmentId)
                                .ToListAsync();
            return tickets != null ? _mapper.Map<List<TicketVM>>(tickets) : null;
        }


    }
}
