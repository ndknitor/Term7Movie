using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieApi.Entities
{
    public class RefreshToken
    {
        public long Id { get; set; }
        [Required]
        public string Value { get; set; }
        [Column(TypeName = "uniqueidentifier"), Required]
        public Guid Jti { get; set; } // check if access token contains it is the same as the refresh token sent to server
        [Required]
        public DateTime ExpiredDate { set; get; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public bool IsRevoked { get; set; } = false;
    }
}
