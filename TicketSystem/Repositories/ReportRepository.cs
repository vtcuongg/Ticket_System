using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Repositories.Interface;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public ReportRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<RatingReportVM>> GetRatingReport(int DepartmentId)
        {
            var result = await(from u in _context.Users
                               join d in _context.Departments on u.DepartmentID equals d.DepartmentID
                               // Lọc theo DepartmentID (truyền vào như tham số)
                               where u.DepartmentID == DepartmentId
                               join tfa in _context.TicketFeedbackAssignees on u.Id equals tfa.AssignedTo into tfaGroup
                               from tfa in tfaGroup.DefaultIfEmpty()
                               join tf in _context.TicketFeedBacks on tfa.TicketID equals tf.TicketID into tfGroup
                               from tf in tfGroup.DefaultIfEmpty()
                               group tf by new { u.Id, u.UserName } into g
                               select new RatingReportVM
                               {
                                   EmployeeID = g.Key.Id,
                                   FullName = g.Key.UserName,
                                   TotalFeedBack = g.Count(x => x != null),
                                   AverageRating = g.Any(x => x != null) ? g.Average(x => (float?)x.Rating) ?? 5.0f : 5.0f
                               }).ToListAsync();

            return result;
        }
        public async Task<object> GetTicketSummary(DateTime? startDate, DateTime? endDate, int departmentId)
        {
            var query = _context.Tickets.AsQueryable();

            // Lọc theo khoảng thời gian nếu có
            if (startDate.HasValue)
            {
                query = query.Where(t => t.CreatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.CreatedAt <= endDate.Value);
            }

            // Lọc theo phòng ban
            query = query.Where(t => t.DepartmentID == departmentId);

            // Lấy tổng số ticket
            var totalTicket = await query.CountAsync();

            // Lấy tổng hợp theo trạng thái
            var ticketSummary = await query
                .GroupBy(t => new { t.Status, Year = t.CreatedAt.Year, Month = t.CreatedAt.Month })
                .Select(g => new SumaryTicketVM
                {
                    Status = g.Key.Status,
                    TicketCount = g.Count(),
                    TicketYear = g.Key.Year,
                    TicketMonth = g.Key.Month
                })
                .OrderByDescending(t => t.TicketYear)
                .ThenByDescending(t => t.TicketMonth)
                .ToListAsync();

            // Nếu ticketSummary null thì gán danh sách trống
            ticketSummary ??= new List<SumaryTicketVM>();

            // Trả kết quả
            return new
            {
                TotalTicket = totalTicket,
                TicketSummary = ticketSummary
            };
        }

        public async Task<object> GetUserSumary(int DepartmentId)
        {
           
            var userSummary = await _context.Users
                .Where(u=>u.DepartmentID== DepartmentId)
                .GroupBy(u => u.Status)
                .Select(g => new UserSumaryVM
                {
                    Status = g.Key,
                    UserCount = g.Count()
                })
                .ToListAsync();
            if (userSummary == null)
            {
                userSummary = new List<UserSumaryVM>();
            }
            var totalUsers = await _context.Users
                 .Where(u => u.DepartmentID == DepartmentId)
                 .CountAsync(); 
            var result = new 
            {
                TotalUsers = totalUsers,
                StatusCounts = userSummary
            };

            return result;
        }
    }
}
