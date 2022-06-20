using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieCore.Entities
{
    public class TicketType
    {
        public long Id { set; get; }
        [Column(TypeName = "nvarchar(50)")]
        public string Name { set; get; }
        public int CompanyId { set; get; }
        public ICollection<ShowtimeTicketType> ShowtimeTicketTypes { set; get; }
    }
}
