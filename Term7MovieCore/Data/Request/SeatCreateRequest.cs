using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace Term7MovieCore.Data.Request
{
    public class SeatCreateRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        //viết regex sau :')
        public string Name { get; set; }

        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_INVALID_FIELD)]
        public int RoomNo { get; set; }

        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_INVALID_FIELD)]
        public int SeatTypeNo { get; set; }

        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_INVALID_FIELD)]
        public int ColumnIndex { get; set; }

        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_INVALID_FIELD)]
        public int RowIndex { get; set; }
    }
}
