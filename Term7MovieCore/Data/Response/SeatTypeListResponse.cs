using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.Response
{
    public class SeatTypeListResponse : ParentResponse
    {
        public IEnumerable<SeatTypeDto> SeatTypes { set; get; }
    }
}
