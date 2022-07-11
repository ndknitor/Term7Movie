namespace Term7MovieCore.Data.Dto
{
    public class ShowtimeTicketTypeDto
    {
        public Guid Id { get; set; }
        public long ShowtimeId { set; get; }
        public ShowtimeDto Showtime { set; get; }
        public long TicketTypeId { set; get; }
        public TicketTypeDto TicketType { set; get; }
        //public decimal OriginalPrice { set; get; }
        public decimal ReceivePrice { set; get; }
    }
}
