using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Response
{
    public class MovieHomePageResponse : ParentResponse
    {
        public int movieID { get; set; }
        public string coverImgURL { get; set; }
    }
}
