using Term7MovieCore.Data.Collections;
using Term7MovieCore.Entities;
using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.Response
{
    public class MovieListResponse : ParentResponse
    {
        public IEnumerable<MovieDTO> MovieList { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; } = 16;
        public int TotalPages { get; set; }
    }
}
