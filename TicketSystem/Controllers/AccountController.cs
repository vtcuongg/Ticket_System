using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Models;
using TicketSystem.Repositories.Interface;
using TicketSystem.ViewModel;

namespace TicketSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;

        public AccountController(IAccountRepository accountRepository, IUserRepository userRepository)
        {
            this._accountRepository = accountRepository;
            this._userRepository = userRepository;
        }
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInVM entity)
        {

            if (entity == null || string.IsNullOrEmpty(entity.Email) || string.IsNullOrEmpty(entity.Password))
            {
                return BadRequest(new { message = "Email và mật khẩu không được để trống." });
            }

            var token = await _accountRepository.SignIn(entity);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Đăng nhập thất bại. Vui lòng kiểm tra lại thông tin." });
            }

            var user = await _userRepository.GetByEmail(entity.Email);
            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng." });
            }

            return Ok(new
            {
                message = "Đăng nhập thành công.",
                user,
                token
            });
        }

    }
}
