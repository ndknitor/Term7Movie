using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieCore.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(50)"), Required]
        public string Name { get; set; }
        public ICollection<MovieCategory> MovieCategories { set; get; }
    }
}
