

using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request.Movie
{
    public class MovieUpdateRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int MovieId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime ReleasedDate { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Duration { get; set; }
        [Range(1, int.MaxValue)]
        [Required]
        public int? RestrictedAge { get; set; }
        public string PosterImgURL { get; set; }
        [Required]
        public string CoverImgURL { get; set; }
        [Required]
        public string TrailerURL { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool isAvailable { get; set; }
        //[Range(1, int.MaxValue)]
        //public int? DirectorId { get; set; } = null;
        [Required]
        public int[] CategoryIDs { get; set; } = null;
        public string Director { get; set; }
        [Required]
        public string[] Actors { get; set; }
        [Required]
        public string[] Language { get; set; }
    }
}
