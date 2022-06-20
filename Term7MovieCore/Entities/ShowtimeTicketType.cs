using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieCore.Entities
{
    public class ShowtimeTicketType
    {
        public Guid Id { get; set; }
        public long ShowtimeId { set; get; }
        public Showtime Showtime { set; get; } 
        public long TicketTypeId { set; get; }
        public TicketType TicketType { set; get; }
        [Column(TypeName = "money")]
        public decimal OriginalPrice { set; get; }
        [Column(TypeName = "money")]
        public decimal ReceivePrice { set; get; }
        public ICollection<Ticket> Tickets { set; get; }
    }
}
