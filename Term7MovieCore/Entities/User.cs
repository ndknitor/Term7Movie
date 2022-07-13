using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Term7MovieCore.Data.Enum;

namespace Term7MovieCore.Entities
{
    public class User : IdentityUser<long>
    {
        [Column(TypeName = "nvarchar(100)")]
        public string FullName { set; get; }
        [Column(TypeName = "nvarchar(200)")]
        public string Address { set; get; }
        [Column(TypeName = "varchar(200)")]
        public string PictureUrl { set; get; }
        [Column(TypeName = "money")]
        public decimal Point { set; get; }
        public int? CompanyId { set; get; }
        public virtual TheaterCompany Company { set; get; }
        [Required]
        public int StatusId { set; get; } = (int) UserStatusEnum.Active;
        public UserStatus Status {set; get;}
        public ICollection<UserRole> UserRoles { set; get; } 
        public ICollection<RefreshToken> RefreshTokens { set; get; }
        public ICollection<TransactionHistory> TransactionHistories { set; get; }
        public ICollection<Theater> Theaters { set; get; }
        public ICollection<PromotionCode> PromotionCodes { set; get; }
        public ICollection<MovieRating> MovieRatings { set; get; }
        public ICollection<TopUpHistory> TopUpHistories { set; get; }
    }
}
