using System.ComponentModel.DataAnnotations;
using Term7MovieCore.Data;

namespace Term7MovieCore.Data.ValidationAttributes
{
    public class CompareEndTimeAttribute : ValidationAttribute
    {
        public string StartTimeProperty { set; get; }
        public CompareEndTimeAttribute(string startTimeProperty)
        {
            StartTimeProperty = startTimeProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime endTime = Convert.ToDateTime(value);

            var property = validationContext.ObjectType.GetProperty(StartTimeProperty);

            if (property == null) throw new ArgumentException();

            DateTime startTime = Convert.ToDateTime(property.GetValue(validationContext.ObjectInstance));

            if (endTime <= startTime) return new ValidationResult(Constants.CONSTRAINT_REQUEST_MESSAGE_END_TIME_NOT_VALID);

            return ValidationResult.Success;
        }
    }
}
