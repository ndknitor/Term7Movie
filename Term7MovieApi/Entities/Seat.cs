using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieApi.Entities
{
    public class Seat
    {
        public long Id { set; get; }
        [Column(TypeName = "varchar(5)"), Required]
        public string Name { set; get; }
        [Required]
        public int RoomId { set; get; }
        public Room Room { set; get; }
        [Required]
        public int ColumnPos { set; get; }
        [Required]
        public int RowPos { set; get; }
        [Required]
        public int SeatTypeId { set; get; }
        public SeatType SeatType { set; get; }
        [JsonIgnore]
        public ICollection<Ticket> Tickets { set; get; }
    }
}
