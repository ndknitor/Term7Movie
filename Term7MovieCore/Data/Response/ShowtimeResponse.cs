using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.Response
{
    public class ShowtimeResponse : ParentResponse
    {
        public ShowtimeDto Showtime { set; get; }
    }
}
