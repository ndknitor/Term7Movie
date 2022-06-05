

using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request
{
    public class MovieActionRequest
    {
        [Required]
        public string Action { get; set; }//why do we have to do this shit anyway

        public int movieId { get; set; }

        public int pageIndex { get; set; }
    }
}
