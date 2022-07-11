using System.ComponentModel.DataAnnotations;
using Term7MovieCore.Data.Request;

namespace Term7MovieCore.Data.ValidationAttributes
{
    public class UniqueTicketTypeListAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            IEnumerable<ShowtimeTicketTypeAdditionCreateRequest> list = value as IEnumerable<ShowtimeTicketTypeAdditionCreateRequest>;

            IEnumerable<ShowtimeTicketTypeAdditionCreateRequest> disctinct = list.Distinct();

            if (list.Count() != disctinct.Count()) return false;

            return true;
        }
    }
}
