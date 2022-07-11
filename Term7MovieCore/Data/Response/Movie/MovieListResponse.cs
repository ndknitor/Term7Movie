using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.Response.Movie
{
    public class MovieListResponse : ParentResponse
    {
        public PagingList<MovieModelDto> Movies { set; get; }
    }
}
