using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request
{
    public class TheaterDefaultPriceUpdateRequest
    {
        [Required]
        public int TheaterId { set; get; }
        [Range(1000, double.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_MIN_RANGE)]
        public decimal DefaultPrice { set; get; }
    }
}
