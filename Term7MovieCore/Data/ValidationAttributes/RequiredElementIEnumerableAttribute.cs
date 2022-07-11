using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.ValidationAttributes
{
    public class RequiredElementIEnumerableAttribute : ValidationAttribute
    {
        public RequiredElementIEnumerableAttribute()
        {
            ErrorMessage = Constants.CONSTRAINT_LIST_CAN_NOT_EMPTY;
        }
        public override bool IsValid(object value)
        {
            IEnumerable<object> list = value as IEnumerable<object>;

            if (list == null || !list.Any()) return false;

            return true;
        }
    }
}
