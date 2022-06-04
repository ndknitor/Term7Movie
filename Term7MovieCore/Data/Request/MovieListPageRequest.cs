using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Request
{
    public class MovieListPageRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; } = 16;
    }
}
