using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request
{
    public class ShowtimeFilterRequest : ParentFilterRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int TheaterId { set; get; }
    }
}
