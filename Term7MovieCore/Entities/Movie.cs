using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieCore.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public int ExternalId { set; get; }
        [Column(TypeName = "nvarchar(300)")]
        public string Title { get; set; }
        public DateTime ReleaseDate { set; get; }
        public int Duration { set; get; }
        public int? RestrictedAge { set; get; }
        [Column(TypeName = "varchar(max)")]
        public string PosterImageUrl { set; get; }
        [Column(TypeName = "varchar(max)")]
        public string CoverImageUrl { set; get; }
        [Column(TypeName = "varchar(max)")]
        public string TrailerUrl { set; get; }
        [Column(TypeName = "nvarchar(max)")]
        public string Description { set; get; }
        public long ViewCount { set; get; }
        public float TotalRating { set; get; }
        public int? DirectorId { set; get; }
        public Director Director { set; get; }
        public ICollection<MovieActor> MovieActors { set; get; }
        public ICollection<MovieLanguage> MovieLanguages { set; get; }
        public ICollection<MovieCategory> MovieCategories { set; get; }
        public ICollection<Showtime> MovieShowtimes { set; get; }
        public ICollection<MovieRating> MovieRatings { set; get; }
    }
}
