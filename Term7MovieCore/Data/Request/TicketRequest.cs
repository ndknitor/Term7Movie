
using Microsoft.AspNetCore.Mvc;

namespace Term7MovieCore.Data.Request
{
    public class TicketRequest
    {
        public int? ShowTimeId { get; set; }
        public Guid? TransactionId { get; set; }
    }
}
