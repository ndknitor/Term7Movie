using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieApi.Entities
{
    public class Language
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(30)"), Required]
        public string Name { get; set; }
        public ICollection<MovieLanguage> MovieLanguages { get; set; }
    }
}
