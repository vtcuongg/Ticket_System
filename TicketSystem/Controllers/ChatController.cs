using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Data;
using TicketSystem.Repositories;
using TicketSystem.Repositories.Interface;
using TicketSystem.Service;
using TicketSystem.ViewModel;

namespace TicketSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatRepository _chatRepository;

        public ChatController(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        [HttpGet("messages")]
        [Authorize]
        public async Task<IActionResult> GetMessages(int userId, int otherUserId)
        {
            var messages = await _chatRepository.GetMessagesAsync(userId, otherUserId);
            return Ok(messages);
        }
        [HttpGet("users-Message/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetChatUsersWithLastMessage(int userId)
        {
            var users = await _chatRepository.GetChatUsersWithLastMessageAsync(userId);
            return Ok(users);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendMessage(SendMessageRequestVM sendMessageRequestVM , IS3Service s3Service)
        {
            try
            {
                await _chatRepository.SendMessageAsync(sendMessageRequestVM,s3Service);
                return Ok(new { message = "Thêm tin nhắn thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thêm Message", error = ex.Message });
            }
        }
    }
}
