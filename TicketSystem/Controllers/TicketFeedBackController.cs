using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Repositories;
using TicketSystem.Repositories.Interface;
using TicketSystem.ViewModel;

namespace TicketSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketFeedBackController : ControllerBase
    {
        private readonly ITicketFeedBackRepository _ticketFeedBackRepository;
        public TicketFeedBackController(ITicketFeedBackRepository ticketFeedBackRepository)
        {
            this._ticketFeedBackRepository = ticketFeedBackRepository;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllTicketFeedBack()
        {
            try
            {
                var ticketFeedBacks = await _ticketFeedBackRepository.GetAll();
                return Ok(new { data = ticketFeedBacks });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách Feedback", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTicketFeedBackById(int id)
        {
            try
            {
                var ticketFeedBack = await _ticketFeedBackRepository.GetById(id);
                if (ticketFeedBack == null)
                    return NotFound(new { message = $"Không tìm thấy Feedback với ID = {id}" });

                return Ok(new { data = ticketFeedBack });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy Feedback", error = ex.Message });
            }
        }

        [HttpGet("ByTicketId/{id}")]
        [Authorize]
        public async Task<IActionResult> GetTicketFeedBackByTicketId(string id)
        {
            try
            {
                var ticketFeedBack = await _ticketFeedBackRepository.GetByTicketId(id);
                if (ticketFeedBack == null)
                    return NotFound(new { message = $"Không tìm thấy Feedback với IDTicket = {id}" });

                return Ok(new { data = ticketFeedBack });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy Feedback", error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTicketFeedBack(TicketFeedBackVM ticketFeedBack)
        {
            try
            {
                if (ticketFeedBack == null)
                    return BadRequest(new { message = "Dữ liệu không hợp lệ" });

                await _ticketFeedBackRepository.Add(ticketFeedBack);
                return Ok(new { message = "Thêm Feedback thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thêm Feedback", error = ex.Message });
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateTicketFeedBack(TicketFeedBackVM ticketFeedBack)
        {
            try
            {
                if (ticketFeedBack == null)
                    return BadRequest(new { message = "Dữ liệu không hợp lệ" });

                await _ticketFeedBackRepository.Update(ticketFeedBack);
                return Ok(new { message = "Cập nhật Feedback thành công" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật Feedback", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTicketFeedBack(int id)
        {
            try
            {

                var ticketFeedBackRepository = await _ticketFeedBackRepository.GetById(id);
                if (ticketFeedBackRepository == null)
                    return NotFound(new { message = $"Không tìm thấy User với ID = {id}" });
                await _ticketFeedBackRepository.Delete(id);
                return Ok(new { message = "Xóa Feedback thành công" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa Feedback", error = ex.Message });
            }
        }
    }
}
