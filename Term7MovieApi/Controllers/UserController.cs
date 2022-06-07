using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data.Response;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<MovieController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser([FromQuery] int id)
        {
            return BadRequest(new ParentResponse { Message = "Chưa dùng"});
        }
    }
}
