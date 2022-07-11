using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request
{
    public class SignInRequest
    {
        [Required]
        public string IdToken { set; get; }
    }
}
