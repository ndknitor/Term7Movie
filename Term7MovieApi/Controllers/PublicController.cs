using Microsoft.AspNetCore.Mvc;
namespace Term7MovieApi;
[ApiController]
[Route("/api/[controller]")]
public class PublicController : Controller
{
    [HttpGet("hello")]
    public ActionResult Test()
    {
        return Ok(new {message = "Hello from F-Cinema"});
    }
}