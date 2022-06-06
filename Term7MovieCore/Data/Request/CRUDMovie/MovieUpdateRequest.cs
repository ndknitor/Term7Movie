

using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request.CRUDMovie
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
        [Range(1, 100)]
        public int? RestrictedAge { get; set; }
        public string PosterImgURL { get; set; }
        public string CoverImgURL { get; set; }
        public string TrailerURL { get; set; }
        [Required]
        public string Description { get; set; }
        [Range(1, int.MaxValue)]
        public int? DirectorId { get; set; }
        public int[] CategoryIDs { get; set; }
    }
}
