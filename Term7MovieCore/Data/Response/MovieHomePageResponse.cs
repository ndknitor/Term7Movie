using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.Response
{
    public class MovieHomePageResponse : ParentResponse
    {
        public IEnumerable<MovieCoverDTO>? movieCoverList { get; set; }
    }
}
