using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace Term7MovieApi;
[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class AuthorizedController : Controller
{

}