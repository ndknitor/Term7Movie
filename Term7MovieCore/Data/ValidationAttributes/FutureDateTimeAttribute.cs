using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.ValidationAttributes
{
    public class FutureDatetimeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dateTime = Convert.ToDateTime(value);

            return dateTime >= DateTime.UtcNow.AddSeconds(-2);
        }
    }
}
