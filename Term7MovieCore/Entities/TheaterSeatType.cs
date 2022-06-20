using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieCore.Entities
{
    public class TheaterSeatType
    {
        public int TheaterId { set; get; }
        public Theater Theater { set; get; }
        public int SeatTypeId { set; get; }
        public SeatType SeatType{ set; get; }
        [Column(TypeName = "money")]
        public decimal BonusPrice { get; set; }
    }
}
