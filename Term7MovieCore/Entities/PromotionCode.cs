using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Entities
{
    public class PromotionCode
    {
        public long Id { get; set; }
        [Required]
        public string Code { set; get; }
        [Required]
        public int DiscountPercent { set; get; }
        [Required]
        public int MaxValue { set; get; }
        [Required]
        public int MinValue { set; get; }
        [Required]
        public int PointCost { set; get; }
        [Required]
        public DateTime AquiredDate { set; get; }
        public DateTime ExpiredDate { set; get; }
        [Required]
        public long CustomerId { get; set; }
        public User Customer { set; get; }
        [Required]
        public int PromotionTypeId { set; get; }
        public PromotionType PromotionType { set; get; }
    }
}
