using System.ComponentModel.DataAnnotations;


namespace Term7MovieCore.Data.Request
{
    public class RoomUpdateRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int No { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int TheaterId { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int NumberOfRow { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int NumberOfColumn { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        public bool Status { set; get; }
    }
}
