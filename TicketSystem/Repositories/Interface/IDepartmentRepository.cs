using TicketSystem.ViewModel;

namespace TicketSystem.Repositories.Interface
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<DepartmentVM>> GetAll();
        Task<DepartmentVM?> GetById(int id);
        Task Add(DepartmentVM entity);
        Task Update(DepartmentVM entity);
        Task Delete(int id);
    }
}
