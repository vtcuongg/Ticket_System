using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace TicketSystem.Models
{
    public class NameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            // Lấy ID từ Claim "NameIdentifier" (thường là user.Id)
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
