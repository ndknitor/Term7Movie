using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieCore.Entities
{
    public class PromotionType
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(256)")]
        public string Name { get; set; }
        [Required]
        public int PercentDiscount { get; set; }
        public ICollection<PromotionCode> PromotionCodes { set; get; }
    }
}
