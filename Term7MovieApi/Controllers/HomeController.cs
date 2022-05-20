using Microsoft.AspNetCore.Mvc;
namespace Term7MovieApi;
[ApiController]
[Route("/api/[controller]")]
public class HomeController : Controller
{
    [HttpGet("hello")]
    public ActionResult Test()
    {
        return Ok(new {message = "Dit me may con cac"});
    }
}