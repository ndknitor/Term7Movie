using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/seats")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly ISeatService _seatService;

        public SeatController(ILogger<MovieController> logger, ISeatService seatService)
        {
            _logger = logger;
            _seatService = seatService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSeat(SeatCreateRequest[] request)
        {
            var response = await _seatService.CreateSeat(request);

            if (response == null)
            {
                return BadRequest(new ParentResponse { Message = Constants.MESSAGE_OPERATION_FAILED });
            }
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSeat(SeatUpdateRequest request)
        {
            var response = await _seatService.UpdateSeat(request);

            if (response == null)
            {
                return BadRequest(new ParentResponse { Message = Constants.MESSAGE_OPERATION_FAILED });
            }
            return Ok(response);
        }

        [HttpDelete("{seatId:long}")]
        public async Task<IActionResult> DeleteSeat(long seatId)
        {
            var response = await _seatService.DeleteSeat(seatId);

            if (response == null)
            {
                return BadRequest(new ParentResponse { Message = Constants.MESSAGE_OPERATION_FAILED });
            }
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoomSeats([FromQuery][Required] int roomId)
        {
            var response = await _seatService.GetRoomSeats(roomId);
            return Ok(response);
        }

        [HttpGet("{seatId:int}")]
        public async Task<IActionResult> GetSeatById(int seatId)
        {
            var response = await _seatService.GetSeatById(seatId);

            if (response == null)
            {
                return NotFound(new ParentResponse { Message = Constants.MESSAGE_NOT_FOUND });
            }

            return Ok(response);
        }
    }
}
