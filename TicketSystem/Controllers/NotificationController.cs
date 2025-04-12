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
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationController(INotificationRepository _notificationRepository)
        {
            this._notificationRepository = _notificationRepository;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNotification(NotificationVM notification)
        {
            try
            {
                if (notification == null)
                    return BadRequest(new { message = "Dữ liệu không hợp lệ" });

                await _notificationRepository.Add(notification);
                return Ok(new { message = "Thêm thông báo  thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thêm thông báo ", error = ex.Message });
            }
        }
        [HttpGet("{UserId}")]
        [Authorize]
        public async Task<IActionResult> GetNotificationsByUserId(int UserId)
        {
            try
            {
                var notifications = await _notificationRepository.GetByUserId(UserId);
                if (notifications == null || !notifications.Any())
                    return NotFound(new { message = $"Không tìm thấy thông báo  nào cho UserID = {UserId}" });

                return Ok(new { data = notifications });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi thêm thông báo ", error = ex.Message });
            }
        }
    }
}
