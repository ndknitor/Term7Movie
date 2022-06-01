using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.Response
{
    public class TheaterRoomsResponse : ParentResponse
    {
        public int TheaterId { set; get; }
        public IEnumerable<RoomDto> Rooms { set; get; }
    }
}
