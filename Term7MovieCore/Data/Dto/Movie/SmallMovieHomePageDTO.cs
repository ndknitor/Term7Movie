using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Dto.Movie
{
    public class SmallMovieHomePageDTO
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string PosterImgURL { get; set; }
        public string CoverImgURL { get; set; }
    }
}
