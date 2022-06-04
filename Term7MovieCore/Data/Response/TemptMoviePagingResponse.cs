using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.Response
{
    public class TemptMoviePagingResponse : ParentResponse
    {
        public IEnumerable<MovieDTO> MovieList { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; } = 16;
        public int TotalPages { get; set; }
    }
}
