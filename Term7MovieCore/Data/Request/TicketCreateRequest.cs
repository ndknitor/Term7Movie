

using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request
{
    public class TicketCreateRequest
    {

        [Required]
        [Range(1, long.MaxValue)]
        public long SeatId { get; set; }
        
        public Guid TransactionId { get; set; }

        [Range(1, long.MaxValue)]
        public long ShowTimeId { get; set; }
        
        [Required]
        public DateTime ShowStartTime { get; set; }
        
        [Required]
        public decimal OriginalPrice { get; set; }
        
        [Required]
        public decimal ReceivePrice { get; set; }
        
        [Required]
        public decimal SellingPrice { get; set; }
        
        [Required]
        public int StatusId { get; set; }
        
        public DateTime LockedTime { get; set; }
    }
}
