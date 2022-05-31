using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Dto
{
    public class MovieHomePageDTO
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public Dictionary<int, string> Categories { get; set; }
        public int? AgeRestrict { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? CoverImgURL { get; set; }
        public string? PosterImgURL { get; set; }
    }
}
