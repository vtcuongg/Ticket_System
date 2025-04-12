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
    public class CategoryController : ControllerBase
    {

        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAllCategory()
        {
            try
            {
                var categories = await _categoryRepository.GetAll();
                return Ok(new { data = categories });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách danh mục", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryRepository.GetById(id);
                if (category == null)
                    return NotFound(new { message = $"Không tìm thấy danh mục với ID = {id}" });

                return Ok(new { data = category });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh mục", error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory(CategoryVM category)
        {
            try
            {
                if (category == null)
                    return BadRequest(new { message = "Dữ liệu không hợp lệ" });

                await _categoryRepository.Add(category);
                return Ok(new { message = "Thêm danh mục thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thêm danh mục", error = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(CategoryVM category)
        {
            try
            {
                if (category == null)
                    return BadRequest(new { message = "Dữ liệu không hợp lệ" });

                await _categoryRepository.Update(category);
                return Ok(new { message = "Cập nhật danh mục thành công" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật danh mục", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _categoryRepository.GetById(id);
                if (category == null)
                    return NotFound(new { message = $"Không tìm thấy danh mục với ID = {id}" });

                await _categoryRepository.Delete(id);
                return Ok(new { message = "Xóa danh mục thành công" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa danh mục", error = ex.Message });
            }
        }
    }
}
