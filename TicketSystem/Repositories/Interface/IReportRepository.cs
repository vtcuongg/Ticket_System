using TicketSystem.ViewModel;

namespace TicketSystem.Repositories.Interface
{
    public interface IReportRepository
    {
        Task<List<RatingReportVM>> GetRatingReport(int DepartmentId);
        Task<object> GetTicketSummary(int? year, int? month, int DepartmentId);
        Task<object> GetUserSumary(int DepartmentId);
    }
}
