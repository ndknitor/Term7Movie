﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieApi.Entities
{
    public class Ticket
    {
        public long Id { get; set; }
        [Required]
        public long SeatId { get; set; }
        public Seat Seat { set; get; }
        public long? TransactionId { get; set; }
        [JsonIgnore]
        public Transaction Transaction { set; get; }
        public long? ShowTimeId { get; set; }
        [JsonIgnore]
        public Showtime ShowTime { set; get; }
        [Required]
        public DateTime ShowStartTime { get; set; }
        [Column(TypeName = "money"), Required]
        public decimal OriginalPrice { get; set; }
        [Column(TypeName = "money"), Required]
        public decimal ReceivePrice { get; set; }
        [Column(TypeName = "money"), Required]
        public decimal SellingPrice { set; get; }
        [Required]
        public int StatusId { set; get; }
        public TicketStatus Status { set; get; }
        public DateTime? LockedTime { set; get; }
    }
}
