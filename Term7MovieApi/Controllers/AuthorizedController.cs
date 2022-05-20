using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace Term7MovieApi;
[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class AuthorizedController : Controller
{
    private readonly IHttpContextAccessor accessor;
    public AuthorizedController(IHttpContextAccessor httpContextAccessor)
    {
        accessor = httpContextAccessor;
    }
    [HttpGet("signout")]
    public async Task<ActionResult> SignOut()
    {
        await accessor.HttpContext.SignOutAsync();
        return Ok(new {message = "Ok"});
    }
}