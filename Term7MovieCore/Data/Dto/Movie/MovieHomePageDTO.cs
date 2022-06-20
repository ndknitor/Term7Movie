
namespace Term7MovieCore.Data.Dto.Movie
{
    public class MovieHomePageDTO
    {//trả 3 phim đề xuất
        //Field show ở home
        public string Title { get; set; }
        public int MovieId { get; set; }
        public string PosterImgURL { get; set; }
        public int TheaterId { get; set; }
        public long ShowTimeId { get; set; }
        //public string ShowTimeId { get; set; }
        public DateTime StartTime { get; set; }
        public string FormattedStartTime { get; set; }
        public int MinutesRemain { get; set; }
        public double DistanceCalculated { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public double RecommendPoint { get; set; }
        //Field cho Detail từng cái ở trên (field này dùng khi người ta click vào)
        public string TheaterName { get; set; }
        //public string TheaterAddress { get; set; }
        //public IEnumerable<ShowtimeDto> Showtimes { get; set; } //3 xuất chiếu sắp xảy ra (hoặc ít hơn nếu thiếu data)
    }
}
