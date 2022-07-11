using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Request
{
    public class ShowtimeTicketTypeAdditionCreateRequest : IEqualityComparer<ShowtimeTicketTypeAdditionCreateRequest>
    {
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        public long TicketTypeId { set; get; }
        //[Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        //[Range(1000, double.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_MIN_RANGE)]
        //public decimal OriginalPrice { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1000, double.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_MIN_RANGE)]
        public decimal ReceivePrice { set; get; }

        public bool Equals(ShowtimeTicketTypeAdditionCreateRequest x, ShowtimeTicketTypeAdditionCreateRequest y)
        {
            if (x == null) return false;

            if (y == null) return false;

            return x.TicketTypeId == y.TicketTypeId;
        }

        public int GetHashCode([DisallowNull] ShowtimeTicketTypeAdditionCreateRequest obj)
        {
            return TicketTypeId.GetHashCode();
        }
    }
}
