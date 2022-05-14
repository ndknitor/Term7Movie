using Microsoft.AspNetCore.Mvc;
namespace Term7MovieApi;
[ApiController]
[Route("/api/[controller]")]
public class HomeController : Controller
{
    [HttpPost("hello")]
    public ActionResult Test(SignInRequest request)
    {
        return Ok(new {fuck = "Dit me may"});
    }
}