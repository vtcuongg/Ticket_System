using Microsoft.AspNetCore.Identity;
using TicketSystem.Data;
using TicketSystem.Models;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserVM>> GetAll();
        Task<UserVM?> GetById(int id);
        Task<IEnumerable<UserVM>> GetByDepartmentId(int id);
        Task<IdentityResult> Add(UserModel entity);
        Task Update(UserVM entity);
        Task Delete(string email);
        Task DeleteById(int id);
        Task<UserVM> GetByEmail(string email);
        Task<UserVM> GetByName(string name);
        Task UpdateStatus(int userId, String newStatus);
        Task<IdentityResult> ChangeUserPassword(int userId, string currentPassword, string newPassword);
    }
}
