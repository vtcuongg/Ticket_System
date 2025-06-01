using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace TicketSystem.Models
{
    public class NameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var userId= connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new Exception("User ID not found in claims.");
            }
            return userId;
        }
    }
}
