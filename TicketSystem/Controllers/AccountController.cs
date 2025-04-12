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
            var result = await _accountRepository.SignIn(entity);
            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }
            var user = await _userRepository.GetByEmail(entity.Email ?? "");
            return Ok(new { message = "Success",User=user, token = result });
        }

    }
}
