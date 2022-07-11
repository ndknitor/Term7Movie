using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieCore.Entities
{
    public class TicketStatus
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(20)"), Required]
        public string Name { get; set; }
    }
}
