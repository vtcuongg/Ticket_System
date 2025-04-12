using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Repositories.Interface;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public RoleRepository(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        // Thêm Role mới
        public async Task<bool> Add(RoleVM entity)
        {
            var role = new Role { Name = entity.Name, NormalizedName = entity.NormalizedName };
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;
        }
        // Xóa Role theo ID
        public async Task<bool> Delete(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return false;

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        // Lấy danh sách tất cả Role
        public async Task<IEnumerable<RoleVM>> GetAll()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            if (roles == null || !roles.Any())
            {
                return new List<RoleVM>(); // Trả về một danh sách trống
            }
            return _mapper.Map<IEnumerable<RoleVM>>(roles);
        }
        // Lấy Role theo ID
        public async Task<RoleVM?> GetById(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            return role != null ? _mapper.Map<RoleVM>(role) : null;
        }

        // Cập nhật Role
        public async Task<bool> Update(RoleVM entity)
        {
            var role = await _roleManager.FindByIdAsync(entity.Id.ToString());
            if (role == null) return false;

            role.Name = entity.Name;
            role.NormalizedName = entity.NormalizedName;
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
    }

}
