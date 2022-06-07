namespace Term7MovieCore.Data.Dto
{
    public class ShowtimeDto
    {
        public long Id { get; set; }
        public int MovieId { set; get; }
        public MovieModelDto Movie { get; set; }
        public int RoomId { set; get; }
        public RoomDto Room { set; get; }
        public int TheaterId { set; get; }
        public DateTime StartTime { set; get; }
        public DateTime EndTime { set; get; }
    }
}
