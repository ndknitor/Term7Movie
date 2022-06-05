using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.Response
{
    public class TheaterListResponse : ParentResponse
    {
        public PagingList<TheaterDto> Theaters { set; get; }
    }
}
