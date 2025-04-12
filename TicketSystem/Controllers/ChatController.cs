using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Repositories.Interface;
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
    }
}
