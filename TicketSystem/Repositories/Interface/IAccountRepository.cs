using Microsoft.AspNetCore.Identity;
using TicketSystem.Models;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories.Interface
{
    public interface IAccountRepository
    {
        Task<string> SignIn(SignInVM entity);

    }
}
