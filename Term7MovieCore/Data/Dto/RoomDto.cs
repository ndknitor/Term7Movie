using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Dto
{
    public class RoomDto
    {
        public int Id { set; get; }
        public int No { set; get; }
        public int TheaterId { set; get; }
        public int NumberOfRow { set; get; }
        public int NumberOfColumn { set; get; }
        public bool Status { set; get; }
        public IEnumerable<SeatDto> SeatDtos { set; get; }
    }
}
