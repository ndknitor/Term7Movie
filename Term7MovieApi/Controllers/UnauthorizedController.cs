using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
namespace Term7MovieApi;
[NonAuthorize]
[ApiController]
[Route("/api/[controller]")]
public class UnauthorazedController : Controller
{
    private readonly IHttpContextAccessor accessor;
    public UnauthorazedController(IHttpContextAccessor httpContextAccessor)
    {
        accessor = httpContextAccessor;
    }
    [HttpPost("signin")]
    public async Task<ActionResult> SignIn(SignInRequest request)
    {
        ClaimsPrincipal principal = new();
        Claim[] claims = new []
        {
            new Claim(ClaimTypes.Role,"User"),
            new Claim(ClaimTypes.NameIdentifier, "1")
        };
        ClaimsIdentity identity = new (claims, CookieAuthenticationDefaults.AuthenticationScheme);
        principal.AddIdentity(identity);
        await accessor.HttpContext.SignInAsync(principal);
        return Ok(new {message = "ok"});
    }
}