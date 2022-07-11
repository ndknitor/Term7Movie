using System;
using System.ComponentModel.DataAnnotations;
using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.Request
{
    public class RoomCreateRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        public int No { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_INVALID_FIELD)]
        public int TheaterId { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_INVALID_FIELD)]
        public int NumberOfRow { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_INVALID_FIELD)]
        public int NumberOfColumn { set; get; }
    }
}
