namespace TicketSystem.ViewModel
{
    public class SendMessageRequestVM
    {
        public int? SenderId { get; set; }
        public int? ReceiverId { get; set; }
        public string? Content { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
