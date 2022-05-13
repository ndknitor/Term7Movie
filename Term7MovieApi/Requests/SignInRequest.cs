using System.ComponentModel.DataAnnotations;

namespace Term7MovieApi;
public record SignInRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email address not in right format")]
    [MinLength(2, ErrorMessage = "Email is too short")]
    [MaxLength(256, ErrorMessage = "Email is too long")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must atleast 6 charaters")]
    [MaxLength(4096, ErrorMessage = "Password is too long")]
    public string Password { get; set; }
}