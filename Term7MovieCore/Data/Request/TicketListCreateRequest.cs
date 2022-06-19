
using System.ComponentModel.DataAnnotations;
using Term7MovieCore.Data.ValidationAttributes;

namespace Term7MovieCore.Data.Request
{
    public class TicketListCreateRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [TicketList(ErrorMessage = Constants.CONSTRAINT_TICKET_LIST_NOT_VALID)]
        public IEnumerable<TicketCreateRequest> Tickets { set; get; }
    }
}
