using Term7MovieCore.Data.Utility;

namespace Term7MovieCore.Data.Dto.Theater
{
    public class TheaterShowTimeLocationDTO
    {
        public long ShowTimeId { get; set; }
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
        public DateTime StartTime { get; set; }
        public Coordinate Location { get; set; }
    }

    public class TheaterShowTimeDTO
    {
        public long ShowTimeId { get; set; }
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public int TheaterId { get; set; }
        public DateTime StartTime { get; set; }
        public double MinutesRemain { get; set; }
        public double DistanceFromUser { get; set; }
        public double RecommendPoint { get; set; }
        public string PosterImageURL { get; set; }
    }
}
