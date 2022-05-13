using Microsoft.AspNetCore.Mvc;

namespace Term7MovieApi;
[NonAuthorize]
[ApiController]
[Route("/api/[controller]")]
public class UnauthorazedController : Controller
{
    
}