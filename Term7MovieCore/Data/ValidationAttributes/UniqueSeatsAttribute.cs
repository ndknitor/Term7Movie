using System.ComponentModel.DataAnnotations;
using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.ValidationAttributes
{
    public class UniqueSeatsAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            IEnumerable<SeatDto> seats = value as IEnumerable<SeatDto>;

           

            return base.IsValid(value);
        }
    }
}
