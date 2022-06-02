using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request
{
    public class MovieListPageRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAIN_REQUEST_MESSAGE_INVALID_FIELD)]
        public int PageIndex { get; set; }

        public int PageSize { get; } = 16;
    }
}
