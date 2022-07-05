namespace Term7MovieCore.Data.Dto.Movie
{
    public class MovieDetailDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ReleaseDate { set; get; }
        public int Duration { set; get; }
        public int? RestrictedAge { set; get; }
        public string PosterImageUrl { set; get; }
        public string CoverImageUrl { set; get; }
        public string TrailerUrl { set; get; }
        public string Description { set; get; }
        public long ViewCount { set; get; }
        public float TotalRating { set; get; }
        public string Director { get; set; }
        public string[] Actors { get; set; }
        public string[] Languages { get; set; }
        public IEnumerable<MovieType> movieTypes { get; set; }
    }
}
