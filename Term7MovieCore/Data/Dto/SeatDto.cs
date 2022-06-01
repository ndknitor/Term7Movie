using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Dto
{
    public class SeatDto
    {
        public long Id { set; get; }
        public string Name { set; get; }
        public int RoomId { set; get; }
        public int ColumnPos { set; get; }
        public int RowPos { set; get; }
        public int SeatTypeId { set; get; }
        public SeatTypeDto SeatType { set; get; }
    }
}
