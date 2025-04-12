using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Repositories.Interface;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories
{
    public class DepartmentRepository : IDepartmentRepository

    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public DepartmentRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task Add(DepartmentVM entity)
        {
            var department = _mapper.Map<Department>(entity);
            await _context.AddAsync(department);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DepartmentVM>> GetAll()
        {
            var departments = await _context.Departments.ToListAsync();
            return _mapper.Map<IEnumerable<DepartmentVM>>(departments);
        }

        public async Task<DepartmentVM?> GetById(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            return department != null ? _mapper.Map<DepartmentVM>(department) : null;
        }

        public async Task Update(DepartmentVM entity)
        {
            var existingDepartment = await _context.Departments.FindAsync(entity.DepartmentID);
            if (existingDepartment != null)
            {
                _mapper.Map(entity, existingDepartment);
                _context.Departments.Update(existingDepartment);
                await _context.SaveChangesAsync();

            }
            else
            {
                throw new KeyNotFoundException($"Không tìm thấy User với ID = {entity.DepartmentID}");
            }
        }
    }
}
