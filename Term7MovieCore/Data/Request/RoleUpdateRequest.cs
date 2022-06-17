using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request
{
    public class RoleUpdateRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        public long UserId { set; get; }
    }
}
