using System.ComponentModel.DataAnnotations;
using Term7MovieCore.Data.Request;

namespace Term7MovieCore.Data.ValidationAttributes
{
    public class TicketListAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            IEnumerable<TicketCreateRequest> tickets = value as IEnumerable<TicketCreateRequest>;

            IEnumerable<long> showtimes = tickets.Select(t => t.ShowTimeId);

            if (showtimes.Count() != 1) return false;

            IEnumerable<DateTime> startTimes = tickets.Select(t => t.ShowStartTime);

            if (startTimes.Count() != 1) return false;

            IEnumerable<TicketCreateRequest> distinct = tickets.Distinct();

            if (tickets.Count() != distinct.Count()) return false;

            return true;
        }
    }
}
