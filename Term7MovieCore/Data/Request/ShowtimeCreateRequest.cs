using System.ComponentModel.DataAnnotations;
using Term7MovieCore.Data.ValidationAttributes;

namespace Term7MovieCore.Data.Request
{
    public class ShowtimeCreateRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int MovieId { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int RoomId { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int TheaterId { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [ShowtimeInNext30MinAtrribute(ErrorMessage = Constants.CONSTRAINT_REQUEST_SHOWTIME_MUST_BE_IN_40MIN)]
        public DateTime StartTime { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        public IEnumerable<ShowtimeTicketTypeAdditionCreateRequest> ShowtimeTicketTypes { set; get; }
    }
}
