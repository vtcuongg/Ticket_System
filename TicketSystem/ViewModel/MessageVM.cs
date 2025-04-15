namespace TicketSystem.ViewModel
{
    public class MessageVM
    {
        public int? MessageID { get; set; }
        public int? SenderID { get; set; }
        public int? ReceiverID { get; set; }
        public string? Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }

        public List<AttachmentVM> Attachments { get; set; } = new();
    }
}
