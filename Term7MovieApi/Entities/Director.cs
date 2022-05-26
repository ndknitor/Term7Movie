using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieApi.Entities
{
    public class Director
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(50)"), Required]
        public string FullName { get; set; }
        public ICollection<Movie> Movies { set; get; }

    }
}
