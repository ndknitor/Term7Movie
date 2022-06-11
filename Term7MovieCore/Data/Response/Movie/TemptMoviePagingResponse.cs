using Term7MovieCore.Data.Dto.Movie;

namespace Term7MovieCore.Data.Response.Movie
{
    public class TemptMoviePagingResponse : ParentResponse
    {
        public IEnumerable<MovieDTO> MovieList { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; } = 16;
        public int TotalPages { get; set; }
    }
}
