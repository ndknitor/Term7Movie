using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Request;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/showtime-ticket-types")]
    [ApiController]
    public class ShowtimeTicketTypeController : ControllerBase
    {
        private readonly IShowtimeTicketTypeService _showtimeTicketTypeService;

        public ShowtimeTicketTypeController(IShowtimeTicketTypeService showtimeTicketTypeService)
        {
            _showtimeTicketTypeService = showtimeTicketTypeService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetShowtimeTicketTypeByShowtimeIdAsync([Required][FromQuery] long showtimeId)
        {
            var response = await _showtimeTicketTypeService.GetShowtimeTicketTypeByShowtimeIdAsync(showtimeId);

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = Constants.ROLE_MANAGER)]
        public async Task<IActionResult> CreateShowtimeTicketTypeAsync([FromBody] ShowtimeTicketTypeCreateRequest request)
        {
            var response = await _showtimeTicketTypeService.CreateShowtimeTicketTypeAsync(request);

            return Ok(response);
        }
    }
}
