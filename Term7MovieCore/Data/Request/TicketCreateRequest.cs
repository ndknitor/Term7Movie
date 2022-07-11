

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
        public Guid ShowtimeTicketTypeId { set; get; }

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
