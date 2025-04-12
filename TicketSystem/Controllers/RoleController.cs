using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TicketSystem.Data;
using TicketSystem.Repositories.Interface;
using TicketSystem.ViewModel;

namespace TicketSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        public RoleController(IRoleRepository roleRepository)
        {
            this._roleRepository = roleRepository;
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _roleRepository.GetAll();
                return Ok(new { data = roles });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách vai trò", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            try
            {
                var role = await _roleRepository.GetById(id);
                if (role == null)
                    return NotFound(new { message = $"Không tìm thấy Role với ID = {id}" });

                return Ok(new { data = role });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy thông tin Role", error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole(RoleVM role)
        {
            try
            {
                if (role == null)
                    return BadRequest(new { message = "Dữ liệu không hợp lệ" });

                await _roleRepository.Add(role);
                return Ok(new { message = "Thêm Role thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thêm Role", error = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(RoleVM role)
        {
            try
            {
                if (role == null)
                    return BadRequest(new { message = "Dữ liệu không hợp lệ" });

                await _roleRepository.Update(role);
                return Ok(new { message = "Cập nhật Role thành công" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật Role", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var role = await _roleRepository.GetById(id);
                if (role == null)
                    return NotFound(new { message = $"Không tìm thấy Role với ID = {id}" });

                await _roleRepository.Delete(id);
                return Ok(new { message = "Xóa Role thành công" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa Role", error = ex.Message });
            }
        }

    }
}
