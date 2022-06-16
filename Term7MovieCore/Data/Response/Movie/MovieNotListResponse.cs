using Term7MovieCore.Data.Dto.Movie;

namespace Term7MovieCore.Data.Response.Movie
{
    public class MovieNotListResponse : ParentResponse
    {
        public IEnumerable<MovieDTO> movieList { get; set; }
    }
}
