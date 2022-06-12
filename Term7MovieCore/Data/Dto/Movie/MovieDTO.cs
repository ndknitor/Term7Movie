using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace Term7MovieCore.Data.Dto.Movie
{
    public class MovieDTO
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public IEnumerable<MovieType> Categories { get; set; }
        public int? AgeRestrict { get; set; }
        public int Duration { get; set; }
        public string ReleaseDate { get; set; }
        public string CoverImgURL { get; set; }
        public string PosterImgURL { get; set; }
    }

    public class MovieType
    {
        public int CateId { get; set; }
        public string CateName { get; set; }
    }
}