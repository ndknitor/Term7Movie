using System.ComponentModel.DataAnnotations;
using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.ValidationAttributes
{
    public class UniqueLongIdListAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            IEnumerable<long> list = value as IEnumerable<long>;

            HashSet<long> set = new HashSet<long>(list);

            if (list.Count() == set.Count) return true;

            return false;
        }
    }
}
