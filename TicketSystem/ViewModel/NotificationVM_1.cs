namespace TicketSystem.ViewModel
{
    public class NotificationVM_1
    {
        public int NotificationID { get; set; }
        public int? SenderID { get; set; }
        public int? ReceiverID { get; set; }
        public string? TicketID { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? SenderName { get; set; }
        public string? SenderAvatar { get; set; }

    }
}
