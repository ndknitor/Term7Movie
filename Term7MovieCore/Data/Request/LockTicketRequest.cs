using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request
{
    public class LockTicketRequest
    {
        [Required]
        public long TicketId { set; get; }
        [Required]
        public long ShowtimeId { set; get; }
    }
}
