using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Entities
{
    public class Showtime
    {
        public long Id { get; set; }
        [Required]
        public int MovieId { set; get; }
        public Movie Movie { get; set; }
        [Required]
        public int RoomId { set; get; }
        public Room Room { set; get; }
        public int? TheaterId { set; get; }
        public Theater Theater { set; get; }
        [Required]
        public DateTime StartTime { set; get; }
        [Required]
        public DateTime EndTime { set; get; }
        public ICollection<ShowtimeTicketType> ShowtimeTicketTypes { set; get; }
    }
}
