using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using TicketSystem.Data;
using TicketSystem.Models;
using TicketSystem.Repositories;
using TicketSystem.Repositories.Interface;
using TicketSystem.Service;
using TicketSystem.ViewModel;

namespace TicketSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly MyDbContext _dbContext;
        public UserController(IUserRepository userRepository, MyDbContext dbContext)
        {
            this._userRepository = userRepository;
            _dbContext = dbContext; 
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAll();
                return Ok(new { data = new { users } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ", error = ex.Message });

            }


        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userRepository.GetById(id);
                if (user == null)
                    return NotFound(new { message = $"Không tìm thấy User với ID = {id}" });

                return Ok(new { data = user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ", error = ex.Message });
            }
        }
        [HttpGet("Email")]
        [Authorize]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmail(email);
                if (user == null)
                    return NotFound(new { message = $"Không tìm thấy User với Email  = {email}" });

                return Ok(new { data = user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ", error = ex.Message });
            }
        }
        [HttpGet("Name")]
        [Authorize]
        public async Task<IActionResult> GetUserByName(string name)
        {
            try
            {
                var user = await _userRepository.GetByName(name);
                if (user == null)
                    return NotFound(new { message = $"Không tìm thấy User với Email  = {name}" });

                return Ok(new { data = user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ", error = ex.Message });
            }
        }
        [HttpGet("ByDepartment/{id}")]
        [Authorize(Roles="Employee,Manager")]
        public async Task<IActionResult> GetUserByDepartmentId(int id)
        {
            try
            {
                var users = await _userRepository.GetByDepartmentId(id);
                if (users == null)
                    return NotFound(new { message = $"Không tìm thấy User với DepartmentID = {id}" });

                return Ok(new { data = new { users } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ", error = ex.Message });
            }
        }
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddUser(UserModel user)
        {
            try
            {
                if (user == null)
                    return BadRequest(new { message = "Dữ liệu không hợp lệ" });

                await _userRepository.Add(user);
                return Ok(new { message = "Thêm user thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thêm user", error = ex.Message });
            }
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(UserVM user)
        {
            try
            {
                if (user == null)
                    return BadRequest(new { message = "Dữ liệu không hợp lệ" });

                await _userRepository.Update(user);
                return Ok(new { message = "Cập nhật user thành công" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật user", error = ex.Message });
            }
        }
        [HttpDelete("ByEmail")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmail(email);
                if (user == null)
                    return NotFound(new { message = $"Không tìm thấy User với Email = {email}" });

                await _userRepository.Delete(email);
                return Ok(new { message = "Xóa user thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa user", error = ex.Message });
            }
        }
        [HttpDelete("DeleteById")] 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            try
            {
                await _userRepository.DeleteById(id);
                return Ok(new { message = "Xóa user thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa user", error = ex.Message });
            }
        }
        [Authorize]
        [HttpPost("update-avatar")]
        public async Task<IActionResult> UpdateAvatar(IFormFile avatar, [FromServices] IS3Service s3Service)
        {
            if (avatar == null || avatar.Length == 0)
                return BadRequest("Avatar is required.");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null) return NotFound();

            var fileName = $"avatars/{userId}/{Guid.NewGuid()}_{avatar.FileName}";
            var avatarUrl = await s3Service.UploadFileAsync(avatar, fileName);

            user.Avatar = avatarUrl;
            await _dbContext.SaveChangesAsync();

            return Ok(new { avatarUrl });
        }

        [Authorize]
        [HttpPatch("update-status")]
        public async Task<IActionResult> UpdateStatus(int userid , string newStatus)
        {
            try
            {
                await _userRepository.UpdateStatus(userid, newStatus);
                return Ok(new { Message = "User status updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the user status.", Detail = ex.Message });
            }
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(int userid ,string currentPass,string newPass,string confirmPass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            if (newPass != confirmPass)
            {
                ModelState.AddModelError("ConfirmNewPassword", "The new password and confirmation password do not match.");
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _userRepository.ChangeUserPassword(
                   userid,
                    currentPass,
                   newPass
                );

                if (result.Succeeded)
                {
                    return Ok(new { Message = "Password changed successfully." });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while changing password.", Detail = ex.Message });
            }
        }
    }
}
