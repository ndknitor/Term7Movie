using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Request
{
    public class RoomCreateRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        public int No { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_INVALID_FIELD)]
        public int TheaterId { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_INVALID_FIELD)]
        public int NumberOfRow { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_INVALID_FIELD)]
        public int NumberOfColumn { set; get; }
    }
}
