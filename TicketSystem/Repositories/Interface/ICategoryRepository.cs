using TicketSystem.ViewModel;

namespace TicketSystem.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryVM>> GetAll();
        Task<CategoryVM?> GetById(int id);
        Task Add(CategoryVM entity);
        Task Update(CategoryVM entity);
        Task Delete(int id);
    }
}
