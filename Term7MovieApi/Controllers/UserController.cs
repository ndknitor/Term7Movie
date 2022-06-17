using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Request;
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

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync([FromQuery] UserFilterRequest request)
        {
            var response = await _userService.GetAllUserAsync(request);
            return Ok(response);
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        [HttpGet("noadmin")]
        public async Task<IActionResult> GetAllUserButAdminIsGoneAsync([FromQuery] UserFilterRequest request)
        {
            var response = await _userService.GetAllUserWithoutAdminAsync(request);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            try
            {
                var result = await _userService.GetUserFromId(userId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(new ParentResponse { Message = "Chụp hình gửi Nam Trần nha huhu. " + ex.Message });
            }
        }

        [Authorize]
        [HttpPut] 
        public async Task<IActionResult> UpdateFullnameForUser(UserRequest request)
        {
            try
            {
                var result = await _userService.UpdateNameForUser(request);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(new ParentResponse { Message = "Chụp hình gửi Nam Trần nha huhu. " + ex.Message });
            }
        }
    }
}
