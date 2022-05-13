using System.ComponentModel.DataAnnotations;
namespace Term7MovieApi;

public class FutureAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true;
        }
        else
        {
            DateTime target = (DateTime) value;
            if (target.ToUniversalTime() < DateTime.UtcNow)
            {
                if (ErrorMessage == null)
                {
                    ErrorMessage = "Expired date time";
                }
                return false;
            }
            return true;

        }
    }
}