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
        public int? Point { set; get; }
        public int? CompanyId { set; get; }
        public TheaterCompany Company { set; get; }
        [Required]
        public int StatusId { set; get; } = (int) UserStatusEnum.Active;
        public UserStatus Status {set; get;}
        public ICollection<UserRole> UserRoles { set; get; } 
        public ICollection<RefreshToken> RefreshTokens { set; get; }
        public ICollection<TransactionHistory> TransactionHistories { set; get; }
        public ICollection<Theater> Theaters { set; get; }
        public ICollection<PromotionCode> PromotionCodes { set; get; }
    }
}
