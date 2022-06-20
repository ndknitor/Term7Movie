using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request
{
    public class CompanyCreateRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [MaxLength(30, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_MAX_LENGTH)]
        public string Name { get; set; }

        [MaxLength(200, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_MAX_LENGTH)]
        [Url]
        public string LogoUrl { set; get; }
    }
}
