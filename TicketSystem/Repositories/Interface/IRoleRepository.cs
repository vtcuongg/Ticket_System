using TicketSystem.ViewModel;

namespace TicketSystem.Repositories.Interface
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RoleVM>> GetAll();
        Task<RoleVM?> GetById(int id);
        Task<bool> Add(RoleVM entity);
        Task<bool> Update(RoleVM entity);
        Task<bool> Delete(int id);

    }
}
