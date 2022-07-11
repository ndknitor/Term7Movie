using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Dto
{
    public class TicketDto
    {
        public long Id { get; set; }
        public long SeatId { get; set; }
        public SeatDto Seat { set; get; }
        public Guid? TransactionId { get; set; }
        public long ShowTimeId { get; set; }
        public ShowtimeDto Showtime { set; get; }
        public DateTime ShowStartTime { get; set; }
        public decimal ReceivePrice { get; set; }
        public decimal SellingPrice { set; get; }
        public int StatusId { set; get; }
        public string StatusName { set; get; }
        public DateTime? LockedTime { set; get; }
        public Guid ShowtimeTicketTypeId { set; get; }
        //public ShowtimeTicketTypeDto ShowtimeTicketType { set; get; }
        public TicketTypeDto TicketType { set; get; }
    }
}
