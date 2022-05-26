﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieCore.Entities
{
    public class SeatType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "money"), Required]
        public decimal BonusPrice { get; set; }
        public ICollection<Seat> Seats { get; set; }
    }
}
