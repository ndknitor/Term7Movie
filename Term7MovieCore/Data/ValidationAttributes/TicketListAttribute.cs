using System.ComponentModel.DataAnnotations;
using Term7MovieCore.Data.Request;

namespace Term7MovieCore.Data.ValidationAttributes
{
    public class TicketListAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            IEnumerable<TicketCreateRequest> tickets = value as IEnumerable<TicketCreateRequest>;

            IEnumerable<TicketCreateRequest> distinct = tickets.Distinct();

            if (tickets.Count() != distinct.Count()) return false;

            return true;
        }
    }
}
