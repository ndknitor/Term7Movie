using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.Response
{
    public class IncomingMovieResponse : ParentResponse
    {
        public IEnumerable<SmallMovieHomePageDTO> LosslessMovieList { get; set; }
    }
}
