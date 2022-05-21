using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieApi.Entities
{
    public class Transaction
    {
        public long Id { get; set; }
        [Required]
        public long CustomerId { set; get; }
        public User Customer { set; get; }
        [Required]
        public DateTime PurchasedDate { set; get; }
        [Column(TypeName = "money"), Required]
        public decimal Total { set; get; }
        [Column(TypeName = "varchar(200)")]
        public string QRCodeUrl { set; get; }
        [Required]
        public int StatusId { get; set; }
        public TransactionStatus Status { set; get; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
