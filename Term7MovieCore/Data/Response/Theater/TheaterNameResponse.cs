
using Term7MovieCore.Data.Dto.Theater;

namespace Term7MovieCore.Data.Response.Theater
{
    public class TheaterNameResponse : ParentResponse
    {
        public IEnumerable<TheaterNameDTO> TheaterNames { get; set; }
    }
}
