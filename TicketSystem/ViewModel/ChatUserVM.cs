namespace TicketSystem.ViewModel
{
    public class ChatUserVM
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public int LastMessageId { get; set; }
        public Boolean IsRead { get; set; }
        public string LastMessageContent { get; set; }
        public DateTime LastMessageTime { get; set; }
        public int LastMessageSenderID { get; set; }
        public bool IsOnline { get; set; } = false;
        public DateTime? LastOnline { get; set; }
    }
}
