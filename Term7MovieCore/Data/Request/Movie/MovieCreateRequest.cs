using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request.Movie
{
    public class MovieCreateRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime ReleasedDate { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Duration { get; set; }
        public int? RestrictedAge { get; set; }
        public string PosterImgURL { get; set; }
        public string CoverImgURL { get; set; }
        public string TrailerURL { get; set; }
        [Required]
        public string Description { get; set; }
        //public int? DirectorId { get; set; }
        [Required]
        public int[] CategoryIDs { get; set; }
        public string Director { get; set; }
        public string[] Actors { get; set; }
        public string Language { get; set; }
    }
}
