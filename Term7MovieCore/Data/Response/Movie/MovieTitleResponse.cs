

using Term7MovieCore.Data.Dto.Movie;

namespace Term7MovieCore.Data.Response.Movie
{
    public class MovieTitleResponse : ParentResponse
    {
        public IEnumerable<MovieTitleDTO> MovieTitles { get; set; }
    }
}
