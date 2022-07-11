using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request
{
    public class ShowtimeTicketTypeCreateRequest : ShowtimeTicketTypeAdditionCreateRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        public long ShowtimeId { set; get; }
    }
}
