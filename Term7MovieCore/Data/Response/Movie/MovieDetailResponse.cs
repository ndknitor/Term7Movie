using Term7MovieCore.Data.Dto.Movie;

namespace Term7MovieCore.Data.Response.Movie
{
    public class MovieDetailResponse : ParentResponse
    {
        public MovieDetailDTO MovieDetail { get; set; }
    }
}
