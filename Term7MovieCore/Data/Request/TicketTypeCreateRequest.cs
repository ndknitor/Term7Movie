using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request
{
    public class TicketTypeCreateRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [MaxLength(50, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_MAX_LENGTH)]
        public string Name { set; get; }
        [Required]
        [Range(1000, double.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_MIN_RANGE)]
        public decimal DefaultPrice { set; get; }
    }
}
