using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Models;
using TicketSystem.Repositories.Interface;
using TicketSystem.ViewModel;

namespace TicketSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketAssignmentController : ControllerBase
    {

        private readonly ITicketAssignmentRepository _ticketAssignmentRepository;
        public TicketAssignmentController(ITicketAssignmentRepository ticketAssignmentRepository)
        {
            this._ticketAssignmentRepository = ticketAssignmentRepository;
        }
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllTicketAssignment()
        {
            try
            {
                var ticketAssignment = await _ticketAssignmentRepository.GetAll();
                return Ok(new { data = ticketAssignment });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách Assingnment", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager,Employee")]
        public async Task<IActionResult> GetTicketAssignmentById(int id)
        {
            try
            {
                var ticketAssignment = await _ticketAssignmentRepository.GetById(id);
                if (ticketAssignment == null)
                    return NotFound(new { message = $"Không tìm thấy Assingnment với ID = {id}" });

                return Ok(new { data = ticketAssignment });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy Assingnment", error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddTicketAssignment(TicketAssignmentVM TicketAssignment)
        {
            try
            {
                if (TicketAssignment == null)
                    return BadRequest(new { message = "Dữ liệu không hợp lệ" });

                await _ticketAssignmentRepository.Add(TicketAssignment);
                return Ok(new { message = "Thêm Assingnment thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thêm Assingnment", error = ex.Message });
            }
        }

        [HttpPost("assign-users")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AssignUsersToTicket([FromBody] AssignUsersRequest request)
        {
            if (request.AssignedToList == null || !request.AssignedToList.Any())
            {
                return BadRequest("Danh sách AssignedTo không được trống.");
            }

            await _ticketAssignmentRepository.AssignUsersToTicket(request.TicketID ?? "", request.AssignedToList);
            return Ok(new { message = "Cập nhật danh sách AssignedTo thành công." });
        }
        [HttpPut]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateTicketAssignment(TicketAssignmentVM TicketAssignment)
        {
            try
            {
                if (TicketAssignment == null)
                    return BadRequest(new { message = "Dữ liệu không hợp lệ" });

                await _ticketAssignmentRepository.Update(TicketAssignment);
                return Ok(new { message = "Cập nhật Assingnment thành công" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật Assingnment", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteTicketAssignment(int id)
        {
            try
            {
                var TicketAssignment = await _ticketAssignmentRepository.GetById(id);
                if (TicketAssignment == null)
                    return NotFound(new { message = $"Không tìm thấy Assingnment với ID = {id}" });

                await _ticketAssignmentRepository.Delete(id);
                return Ok(new { message = "Xóa Assingnment thành công" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa Assingnment", error = ex.Message });
            }
        }
    }
}
