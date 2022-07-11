using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.ValidationAttributes
{
    public class ShowtimeInNext30MinAtrribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime startTime = Convert.ToDateTime(value);
            DateTime now = DateTime.UtcNow;
            return startTime <= now.AddMinutes(Constants.CREATE_SHOWTIME_UPPER_BOUND_IN_MINUTE) &&
                startTime > now;
        }
    }
}
