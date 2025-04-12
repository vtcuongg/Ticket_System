using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Repositories.Interface;
using TicketSystem.ViewModel;

namespace TicketSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketFeedBackAssigneeController : ControllerBase
    {
        private readonly ITicketFeedBackAssigneeRepository _ticketFeedBackAssigneeRepository;
        public TicketFeedBackAssigneeController(ITicketFeedBackAssigneeRepository ticketFeedBackAssigneeRepository)
        {
            this._ticketFeedBackAssigneeRepository = ticketFeedBackAssigneeRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTicketFeedBackAssignee(TicketFeedBackAssigneeVM ticketFeedBackAssignee)
        {

            try
            {
                if (ticketFeedBackAssignee == null)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                await _ticketFeedBackAssigneeRepository.Add(ticketFeedBackAssignee);
                return Ok(new { message = "Thêm thành công." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ nội bộ.", error = ex.Message });
            }
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(string TicketId, int assignedTo)
        {
            try
            {
                if (TicketId ==null || assignedTo <= 0)
                {
                    return BadRequest(new { message = "ID phản hồi hoặc ID người được giao không hợp lệ." });
                }

                await _ticketFeedBackAssigneeRepository.Delete(TicketId, assignedTo);
                return Ok(new { message = "Xóa thành công." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ nội bộ.", error = ex.Message });
            }
        
    }
    }
}
