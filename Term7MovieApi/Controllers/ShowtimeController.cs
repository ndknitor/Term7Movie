using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Extensions;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/showtimes")]
    [ApiController]
    [Authorize]
    public class ShowtimeController : ControllerBase
    {
        private readonly IShowtimeService _showtimeService;

        public ShowtimeController(IShowtimeService showtimeService)
        {
            _showtimeService = showtimeService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetShowtimesAsync([FromQuery] ShowtimeFilterRequest request)
        {
            long userId = Convert.ToInt64(User.Claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));
            ShowtimeListResponse response = await _showtimeService.GetShowtimesAsync(request, userId);
            return Ok(response);
        }

        [HttpGet("{showtimeId:long}")]
        public async Task<IActionResult> GetShowtimeByIdAsync(long showtimeId)
        {
            var response = await _showtimeService.GetShowtimeByIdAsync(showtimeId);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = Constants.ROLE_MANAGER, Policy = Constants.POLICY_CREATE_SHOWTIME_SAME_MANAGER)]
        public async Task<IActionResult> CreateShowtimeAsync(ShowtimeCreateRequest request)
        {
            long managerId = Convert.ToInt64(User.Claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));

            var response = await _showtimeService.CreateShowtimeAsync(request, managerId);
            return Ok(response);
        }

        [HttpPatch] // chua viet policy
        [Authorize(Roles = Constants.ROLE_MANAGER)]
        public async Task<IActionResult> UpdateShowtimeAsync(ShowtimeUpdateRequest request)
        {
            var response = await _showtimeService.UpdateShowtimeAsync(request);
            return Ok(response);
        }
    }
}
