using System.ComponentModel.DataAnnotations;

namespace Term7MovieApi.Requests
{
    public class TokenRequest
    {
        [Required]
        public string AccessToken { set; get; }
        [Required]
        public string RefreshToken { set; get; }
    }
}
