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
        [Required]
        public int? RestrictedAge { get; set; }
        [Required]
        public string PosterImgURL { get; set; }
        [Required]
        public string CoverImgURL { get; set; }
        [Required]
        public string TrailerURL { get; set; }
        [Required]
        public string Description { get; set; }
        //public int? DirectorId { get; set; }
        [Required]
        public int[] CategoryIDs { get; set; }
        [Required]
        public string Director { get; set; }
        [Required]
        public string[] Actors { get; set; }
        [Required]
        public string[] Languages { get; set; }
    }
}
