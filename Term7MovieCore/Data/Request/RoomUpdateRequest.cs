using System.ComponentModel.DataAnnotations;


namespace Term7MovieCore.Data.Request
{
    public class RoomUpdateRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int Id { get; set; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int No { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int NumberOfRow { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int NumberOfColumn { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        public bool Status { set; get; }
    }
}
