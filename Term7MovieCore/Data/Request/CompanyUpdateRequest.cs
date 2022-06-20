using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request
{
    public class CompanyUpdateRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        public int Id { set; get; }
        [MaxLength(30, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_MAX_LENGTH)]
        public string Name { set; get; }
        [MaxLength(200, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_MAX_LENGTH)]
        [Url]
        public string LogoUrl { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        public bool IsActive { set; get; } = true;
    }
}
