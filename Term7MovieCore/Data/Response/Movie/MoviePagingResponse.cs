using Term7MovieCore.Data.Dto.Movie;

namespace Term7MovieCore.Data.Response.Movie
{
    public class MoviePagingResponse : ParentResponse
    {
        public IEnumerable<MovieDTO> MovieList { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
