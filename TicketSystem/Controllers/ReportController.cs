using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Repositories.Interface;

namespace TicketSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        public ReportController(IReportRepository reportRepository)
        {
            this._reportRepository = reportRepository;
        }
        [HttpGet("RatingReport")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetRatingReport(int DepartmentId)
        {
          var ratingReport=   await _reportRepository.GetRatingReport(DepartmentId);
            return Ok(ratingReport);

        }

        [HttpGet("SumaryTicket")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetSumaryTicket(DateTime? startDate, DateTime? endDate, int DepartmentId)
        {
            var sumaryTicket = await _reportRepository.GetTicketSummary(startDate, endDate, DepartmentId);
            return Ok(sumaryTicket);

        }
        [HttpGet("SumaryUser")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetSumaryUser(int DepartmentId)
        {
            var sumaryUser = await _reportRepository.GetUserSumary(DepartmentId);
            return Ok(sumaryUser);

        }
    }
}
