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
            var rawData = await (
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
                 select new
                 {
                     u.Id,
                     u.UserName,
                     Feedback = tf,
                     Assignment = ta,
                     Ticket = t
                 }
             ).ToListAsync(); 

          
            var result = rawData
                .GroupBy(x => new { x.Id, x.UserName })
                .Select(g =>
                {
                    var uniqueFeedbacks = g
                        .Where(x => x.Feedback != null)
                        .Select(x => new { x.Feedback.TicketID, x.Feedback.Rating })
                        .Distinct();

                    var assignedTicketIds = g
                        .Where(x => x.Assignment != null && x.Assignment.TicketID != null)
                        .Select(x => x.Assignment.TicketID!)
                        .Distinct()
                        .ToList();

                    var totalAssigned = assignedTicketIds.Count;

                    var totalValidAssigned = g
                        .Where(x => x.Ticket != null && x.Ticket.Status != "Đã hủy")
                        .Select(x => x.Ticket.TicketID)
                        .Distinct()
                        .Count();

                    var completedOnTime = g
                        .Where(x => x.Ticket != null &&
                                    x.Ticket.Status == "Hoàn thành" &&
                                    x.Ticket.UpdatedAt <= x.Ticket.DueDate)
                        .Select(x => x.Ticket.TicketID)
                        .Distinct()
                        .Count();

                    return new RatingReportVM
                    {
                        EmployeeID = g.Key.Id,
                        FullName = g.Key.UserName,

                        TotalFeedBack = uniqueFeedbacks.Count(),
                        AverageRating = uniqueFeedbacks.Any()
                            ? uniqueFeedbacks.Average(x => (float?)x.Rating) ?? 5.0f
                            : 5.0f,
                        PositiveFeedbackCount = uniqueFeedbacks.Count(x => x.Rating >= 4),

                        TotalAssignedTickets = totalAssigned,

                        TotalOverdueTickets = g
                            .Where(x => x.Ticket != null &&
                                        x.Ticket.DueDate < DateTime.Now &&
                                        x.Ticket.Status != "Hoàn thành" &&
                                        x.Ticket.Status != "Đã hủy" &&
                                        x.Ticket.Status != "Chờ xác nhận")
                            .Select(x => x.Ticket.TicketID)
                            .Distinct()
                            .Count(),

                        TotalCompletedTickets = g
                            .Where(x => x.Ticket != null && x.Ticket.Status == "Hoàn thành")
                            .Select(x => x.Ticket.TicketID)
                            .Distinct()
                            .Count(),

                        TotalInProgressTickets = g
                            .Where(x => x.Ticket != null && (x.Ticket.Status == "Đang xử lý" || x.Ticket.Status == "Chờ xác nhận"))
                            .Select(x => x.Ticket.TicketID)
                            .Distinct()
                            .Count(),

                        CompletionOnTimeRate = totalValidAssigned == 0
                            ? 0
                            : Math.Round(100.0 * completedOnTime / totalValidAssigned, 2)
                    };
                })
                .ToList();

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
