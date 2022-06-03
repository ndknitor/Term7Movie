using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.Response
{
    public class MovieListResponse : ParentResponse
    {
        public PagingList<MovieDto>? Movies { set; get; }
    }
}
