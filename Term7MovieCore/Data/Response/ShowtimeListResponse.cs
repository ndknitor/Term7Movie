using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.Response
{
    public class ShowtimeListResponse : ParentResponse
    {
        public PagingList<ShowtimeDto> Showtimes { set; get; }
    }
}
