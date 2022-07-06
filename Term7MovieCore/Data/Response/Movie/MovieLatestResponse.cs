using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto.Movie;

namespace Term7MovieCore.Data.Response.Movie
{
    public class MovieLatestResponse : ParentResponse
    {
        public PagingList<MovieDTO> movieList { get; set; }
    }
}
