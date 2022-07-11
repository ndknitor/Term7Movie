﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieCore.Entities
{
    public class TransactionHistory
    {
        public long Id { get; set; }
        public Guid TransactionId { get; set; }
        public long UserId { set; get; }
        public User User { get; set; }
        public int MovieId { set; get; }
        public long TicketId { set; get; }
        [Column(TypeName = "money")]
        public decimal TicketPrice { set; get; }
        public int TheaterId { set; get; }
        public string TheaterName { set; get; }
        public DateTime PurchasedDate { set; get; }
    }
}
