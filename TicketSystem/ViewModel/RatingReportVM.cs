namespace TicketSystem.ViewModel
{
    public class RatingReportVM
    {
        public int EmployeeID { get; set; }
        public string FullName { get; set; }
        public int TotalFeedBack { get; set; }
        public float AverageRating { get; set; }
        public int PositiveFeedbackCount { get; set; }
        public int TotalAssignedTickets { get; set; }
        public int TotalOverdueTickets { get; set; }
        public int TotalCompletedTickets { get; set; }
        public int TotalInProgressTickets { get; set; }
        public double CompletionOnTimeRate { get; set; }

    }
}
