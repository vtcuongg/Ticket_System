using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Data;
using TicketSystem.Repositories.Interface;
using TicketSystem.ViewModel;

namespace TicketSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            this._departmentRepository = departmentRepository;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllDepartments()
        {
            try
            {
                var departments = await _departmentRepository.GetAll();
                return Ok(new { data = departments });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách phòng ban", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            try
            {
                var department = await _departmentRepository.GetById(id);
                if (department == null)
                    return NotFound(new { message = $"Không tìm thấy phòng ban với ID = {id}" });

                return Ok(new { data = department });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy thông tin phòng ban", error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDepartment(DepartmentVM department)
        {
            try
            {
                if (department == null)
                    return BadRequest(new { message = "Dữ liệu không hợp lệ" });

                await _departmentRepository.Add(department);
                return Ok(new { message = "Thêm phòng ban thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thêm phòng ban", error = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDepartment(DepartmentVM department)
        {
            try
            {
                if (department == null)
                    return BadRequest(new { message = "Dữ liệu không hợp lệ" });

                await _departmentRepository.Update(department);
                return Ok(new { message = "Cập nhật phòng ban thành công" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật phòng ban", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var department = await _departmentRepository.GetById(id);
                if (department == null)
                    return NotFound(new { message = $"Không tìm thấy phòng ban với ID = {id}" });

                await _departmentRepository.Delete(id);
                return Ok(new { message = "Xóa phòng ban thành công" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa phòng ban", error = ex.Message });
            }
        }
    }
}
