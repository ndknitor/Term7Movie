using System.ComponentModel.DataAnnotations;
using Term7MovieCore.Data.ValidationAttributes;

namespace Term7MovieCore.Data.Request
{
    public class ShowtimeUpdateRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, long.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public long Id { set; get; }

        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [FutureDatetime(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_START_TIME_NOT_VALID)]
        public DateTime StartTime { set; get; }
    }
}
