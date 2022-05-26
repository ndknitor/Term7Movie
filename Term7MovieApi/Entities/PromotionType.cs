using System.ComponentModel.DataAnnotations;

namespace Term7MovieApi.Entities
{
    public class PromotionType
    {
        public int Id { get; set; }
        [Required]
        public int Name { get; set; }
        [Required]
        public int PercentDiscount { get; set; }
        public ICollection<PromotionCode> PromotionCodes { set; get; }
    }
}
