﻿
using Term7MovieCore.Data.Dto.Movie;

namespace Term7MovieCore.Data.Response.Movie
{
    public class MovieHomePageResponse : ParentResponse
    {
        public IEnumerable<MovieHomePageDTO> MovieHomePages { get; set; }
    }
}
