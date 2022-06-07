using Term7MovieCore.Data.Dto.Errors;

namespace Term7MovieCore.Data.Response.Movie
{
    public class MovieCreateResponse : ParentResponse
    {
        public IEnumerable<CreateMovieError> Reports { get; set; }
    }
}
