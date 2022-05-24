

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieApi.Entities
{
    public class Theater
    {
        public int Id { set; get; }
        [Column(TypeName = "nvarchar(50)"), Required]
        public string Name { set; get; }
        [Column(TypeName = "nvarchar(100)"), Required]
        public string Address { set; get; }
        [Required]
        public int CompanyId { set; get; }
        public TheaterCompany Company { set; get; }
        [Required]
        public long ManagerId { set; get; }
        public User Manager { set; get; }
        public bool Status { set; get; }
        [JsonIgnore]
        public ICollection<Room> Rooms { set; get; }
    }
}
