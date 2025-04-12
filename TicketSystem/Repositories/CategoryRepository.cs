using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Repositories.Interface;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task Add(CategoryVM entity)
        {
            var category = _mapper.Map<Category>(entity);
            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CategoryVM>> GetAll()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<IEnumerable<CategoryVM>>(categories);
        }

        public async Task<CategoryVM?> GetById(int id)
        {
            var catogory = await _context.Categories.FindAsync(id);
            return catogory != null ? _mapper.Map<CategoryVM>(catogory) : null;
        }

        public async Task Update(CategoryVM entity)
        {
            var existingCategory = await _context.Categories.FindAsync(entity.CategoryID);
            if (existingCategory != null)
            {
                _mapper.Map(entity, existingCategory);
                _context.Categories.Update(existingCategory);
                await _context.SaveChangesAsync();

            }
            else
            {
                throw new KeyNotFoundException($"Không tìm thấy User với ID = {entity.CategoryID}");
            }
        }
    
}
}
