

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Term7MovieCore.Data.ValidationAttributes;

namespace Term7MovieCore.Data.Request
{
    public class TicketCreateRequest : IEqualityComparer<TicketCreateRequest>
    {
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        public long SeatId { get; set; }

        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        public long ShowTimeId { get; set; }

        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [FutureDatetime(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_START_TIME_NOT_VALID)]
        public DateTime ShowStartTime { get; set; }
        
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1000, double.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_MIN_RANGE)]
        public decimal OriginalPrice { get; set; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1000, double.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_MIN_RANGE)]
        public decimal ReceivePrice { get; set; }

        public bool Equals(TicketCreateRequest x, TicketCreateRequest y)
        {
            return x.SeatId == y.SeatId;
        }

        public int GetHashCode([DisallowNull] TicketCreateRequest obj)
        {
            return SeatId.GetHashCode();
        }
    }
}
