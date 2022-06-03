
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Dto
{
    public class MovieDto
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int ExternalId { set; get; }
        public string Title { get; set; }
        public DateTime ReleaseDate { set; get; }
        public int Duration { set; get; }
        public int? RestrictedAge { set; get; }
        public string PosterImageUrl { set; get; }
        public string CoverImageUrl { set; get; }
        public string TrailerUrl { set; get; }
        public string Description { set; get; }
        public long ViewCount { set; get; }
        public float TotalRating { set; get; }
        public int? DirectorId { set; get; }
    }
}