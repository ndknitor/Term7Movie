using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.Response
{
    public class MovieHomePageResponse : ParentResponse
    {
        public IEnumerable<MovieHomePageDTO>? movieList { get; set; }
    }
}
