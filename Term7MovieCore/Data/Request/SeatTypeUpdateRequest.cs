using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Term7MovieCore.Data.Enum;

namespace Term7MovieCore.Data.Request
{
    public class SeatTypeUpdateRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range((int)SeatTypeEnum.Normal, (int)SeatTypeEnum.Vip, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_INVALID_FIELD)]
        public int Id { get; set; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        public string Name { set; get; }
    }
}
