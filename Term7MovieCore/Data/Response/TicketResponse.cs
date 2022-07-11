
using Term7MovieCore.Data.Dto.Ticket;

namespace Term7MovieCore.Data.Response
{
    public class TicketResponse : ParentResponse
    {
        public IEnumerable<TicketDTO> Tickets { get; set; }
    }
}
