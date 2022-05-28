using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieCore.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(100)"), Required]
        public string Title { get; set; }
        [Required]
        public DateTime ReleaseDate { set; get; }
        public int? RestrictedAge { set; get; }
        [Column(TypeName = "varchar(200)")]
        public string PosterImageUrl { set; get; }
        [Column(TypeName = "varchar(200)")]
        public string CoverImageUrl { set; get; }
        [Column(TypeName = "varchar(200)")]
        public string TrailerUrl { set; get; }
        [Column(TypeName = "nvarchar(200)")]
        public string Description { set; get; }
        [Required]
        public int DirectorId { set; get; }
        public Director Director { set; get; }
        public ICollection<MovieActor> MovieActors { set; get; }
        public ICollection<MovieLanguage> MovieLanguages { set; get; }
        public ICollection<MovieCategory> MovieCategories { set; get; }
        public ICollection<Showtime> MovieShowtimes { set; get; }
    }
}
