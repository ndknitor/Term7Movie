using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Term7MovieCore.Data.Enum;
#nullable disable

namespace Term7MovieCore.Data.Request
{
    public class SeatCreateRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        [MaxLength(5, ErrorMessage = Constants.CONSTRANT_REQUEST_MESSAGE_MAX_LENGTH)]
        public string Name { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int RoomId { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int ColumnPos { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int RowPos { set; get; }

        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        [Range((int)SeatTypeEnum.Normal, (int)SeatTypeEnum.Vip, ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_INVALID_FIELD)]
        public int SeatTypeId { set; get; }
    }
}
