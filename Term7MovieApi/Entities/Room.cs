using System.ComponentModel.DataAnnotations;


namespace Term7MovieApi.Entities
{
    public class Room
    {
        public int Id { set; get; }
        [Required]
        public int No { set; get; }
        [Required]
        public int TheaterId { set; get; }
        public Theater Theater { set; get; }
        [Required]
        public int NumberOfRow { set; get; }
        [Required]
        public int NumberOfColumn { set; get; }
        public bool Status { set; get; }
        public ICollection<Seat> Seats { set; get; }
        public ICollection<Showtime> Showtimes { set; get; }
    }
}
