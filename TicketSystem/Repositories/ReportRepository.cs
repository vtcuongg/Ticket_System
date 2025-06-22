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
            var result = await (
       from u in _context.Users
       join d in _context.Departments on u.DepartmentID equals d.DepartmentID
       where u.DepartmentID == DepartmentId
       join tfa in _context.TicketFeedbackAssignees on u.Id equals tfa.AssignedTo into tfaGroup
       from tfa in tfaGroup.DefaultIfEmpty()
       join tf in _context.TicketFeedBacks on tfa.TicketID equals tf.TicketID into tfGroup
       from tf in tfGroup.DefaultIfEmpty()
       join ta in _context.TicketAssignments on u.Id equals ta.AssignedTo into taGroup
       from ta in taGroup.DefaultIfEmpty()
       join t in _context.Tickets on ta.TicketID equals t.TicketID into tGroup
       from t in tGroup.DefaultIfEmpty()
       group new { tf, t, ta } by new { u.Id, u.UserName } into g
       select new RatingReportVM
       {
           EmployeeID = g.Key.Id,
           FullName = g.Key.UserName,

           TotalFeedBack = g.Count(x => x.tf != null),
           AverageRating = g.Any(x => x.tf != null)
               ? g.Average(x => (float?)x.tf.Rating) ?? 5.0f
               : 5.0f,
           PositiveFeedbackCount = g.Count(x => x.tf != null && x.tf.Rating >= 4),

           TotalAssignedTickets = g
                .Where(x => x.ta != null && x.ta.TicketID != null)
                .Select(x => x.ta.TicketID!)
                .Distinct()
                .Count(),

           TotalOverdueTickets = g
               .Where(x => x.t != null &&
                           x.t.DueDate < DateTime.Now &&
                           x.t.Status != "Hoàn thành" &&
                           x.t.Status != "Đã hủy" &&
                           x.t.Status != "Chờ xác nhận")
               .Select(x => x.t.TicketID)
               .Distinct()
               .Count(),

           TotalCompletedTickets = g
               .Where(x => x.t != null && x.t.Status == "Hoàn thành")
               .Select(x => x.t.TicketID)
               .Distinct()
               .Count(),

           TotalInProgressTickets = g
               .Where(x => x.t != null && x.t.Status == "Đang xử lý")
               .Select(x => x.t.TicketID)
               .Distinct()
               .Count(),

           // ✅ Completion Rate - Đúng hạn, loại bỏ ticket bị hủy
           CompletionOnTimeRate =
               g.Where(x => x.t != null && x.t.Status != "Đã hủy")
                .Select(x => x.t.TicketID)
                .Distinct()
                .Count() == 0
               ? 0
               : Math.Round(
                   100.0 *
                   g.Where(x => x.t != null &&
                                x.t.Status == "Hoàn thành" &&
                                x.t.UpdatedAt <= x.t.DueDate)
                    .Select(x => x.t.TicketID)
                    .Distinct()
                    .Count()
                   /
                   (double)
                   g.Where(x => x.t != null && x.t.Status != "Đã hủy")
                    .Select(x => x.t.TicketID)
                    .Distinct()
                    .Count(),
                   2
               )
       }
   ).ToListAsync();

            return result;
        }
        public async Task<object> GetTicketSummary(DateTime? startDate, DateTime? endDate, int departmentId)
        {
            var query = _context.Tickets.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(t => t.CreatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.CreatedAt <= endDate.Value);
            }

         
            query = query.Where(t => t.DepartmentID == departmentId);

        
            var totalTicket = await query.CountAsync();

          
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

           
            ticketSummary ??= new List<SumaryTicketVM>();

          
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
