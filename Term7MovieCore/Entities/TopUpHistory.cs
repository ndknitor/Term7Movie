using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieCore.Entities
{
    public class TopUpHistory
    {
        public Guid Id { get; set; }
        public long UserId { set; get; }
        public User User { get; set; }
        public DateTime RecordDate { set; get; }
        [Column(TypeName = "money")]
        public decimal Amount { set; get; }
        [Column(TypeName = "nvarchar(256)")]
        public string Description { set; get; }
        public Guid? TransactionId { set; get; }
    }
}
