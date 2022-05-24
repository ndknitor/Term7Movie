using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieApi.Entities
{
    public class TransactionHistory
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public long UserId { set; get; }
        public User User { get; set; }
        public long TicketId { set; get; }
        [Column(TypeName = "money")]
        public decimal TicketPrice { set; get; }
        public int TheaterId { set; get; }
        public string TheaterName { set; get; }
        public DateTime PurchasedDate { set; get; }
    }
}
