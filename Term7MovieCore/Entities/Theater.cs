using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieCore.Entities
{
    public class Theater
    {
        public int Id { set; get; }
        [Column(TypeName = "nvarchar(100)"), Required]
        public string Name { set; get; }
        [Column(TypeName = "nvarchar(200)"), Required]
        public string Address { set; get; }
        [Column(TypeName = "varchar(20)")]
        public string Latitude { set; get; } // North to South
        [Column(TypeName = "varchar(20)")]
        public string Longitude { set; get; } // West to East
        [Required]
        public int CompanyId { set; get; }
        public TheaterCompany Company { set; get; }
        [Required]
        public long ManagerId { set; get; }
        public User Manager { set; get; }
        public bool Status { set; get; }
        public ICollection<Room> Rooms { set; get; }
    }
}
