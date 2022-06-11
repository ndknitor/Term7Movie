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
        public string DirectoryName { get; set; } = "Trần Hào Nam";
        public string[] Actors { get; set; } = new string[] { "Trần Nam", "Nam Trần", "Tram Nần", "Nần Tram" };
        public string SelfDestructMessage { get; } = "Cuối tuần rồi tôi không rảnh ngồi mò data " +
            "actor đâu, tự thân vận động đi (Bức thư này sẽ tự hủy khi có data hoàn chỉnh cho director và actors).";
    }

    public class MovieType
    {
        public int CateId { get; set; }
        public string CateName { get; set; }
    }
}