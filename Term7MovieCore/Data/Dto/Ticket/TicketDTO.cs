using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Dto.Ticket
{
    public class TicketDTO
    {//the comment was for later joining query
        public long TicketId { get; set; }
        public long SeatId { get; set; }
        //Start transaction
        public Guid? TransactionId { get; set; }
        //End transaction
        //Start Showtime
        public long? ShowTimeId { get; set; }
        public DateTime ShowStartTime { get; set; }
        //End Showtime
        public decimal OriginalPrice { get; set; }
        public decimal ReceivePrice { get; set; }
        public decimal SellingPrice { get; set; }
        //Start TicketStatus
        public int StatusId { get; set; }
        public DateTime? LockedTime { get; set; }
        //End TicketStatus
    }
}
