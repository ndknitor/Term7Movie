using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieApi.Entities
{
    public class TheaterCompany
    {
        public int Id { set; get; }
        [Column(TypeName = "nvarchar(30)"), Required]
        public int Name { set; get; }
        [Column(TypeName = "varchar(200)")]
        public string LogoUrl { set; get; }
        public bool IsActive { set; get; }
        [JsonIgnore]
        public ICollection<User> Managers { get; set; }
        public ICollection<Theater> Theaters { set; get; }
    }
}
