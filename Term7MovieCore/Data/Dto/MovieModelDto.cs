namespace Term7MovieCore.Data.Dto
{
    public class MovieModelDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? ReleaseDate { set; get; }
        public int Duration { set; get; }
        public int? RestrictedAge { set; get; }
        public string PosterImageUrl { set; get; }
        public string CoverImageUrl { set; get; }
        public string TrailerUrl { set; get; }
        public string Description { set; get; }
        public long? ViewCount { set; get; }
        public float? TotalRating { set; get; }
        public int? DirectorId { set; get; }
    }
}
