
using Microsoft.AspNetCore.Mvc;

namespace Term7MovieCore.Data.Request
{
    public class TicketRequest
    {
        [FromQuery(Name = "show-time-id")]
        public int? ShowTimeId { get; set; }

        [FromQuery(Name = "transaction-id")]
        public Guid? TransactionId { get; set; }
    }
}
