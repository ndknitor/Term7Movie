namespace Term7MovieCore.Data.Request
{
    public class TicketFilterRequest : ParentFilterRequest
    {
        public long? ShowtimeId { set; get; }
        public long? TicketId { set; get; }
        public bool IsNotShowedYet { set; get; } = false;
        public bool IsPurchased { set; get; } = false;
    }
}
