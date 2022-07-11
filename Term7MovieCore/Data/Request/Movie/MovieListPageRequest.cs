using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Request.Movie
{
    public class MovieListPageRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string TitleSearch { get; set; }
    }
}
