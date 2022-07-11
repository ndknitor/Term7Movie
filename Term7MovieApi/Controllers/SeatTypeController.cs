using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/seat-types")]
    [ApiController]
    public class SeatTypeController : ControllerBase
    {
        private readonly ISeatTypeService _seatTypeService;
        public SeatTypeController(ISeatTypeService seatTypeService)
        {
            _seatTypeService = seatTypeService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllSeatTypeAsync()
        {
            var response = await _seatTypeService.GetAllSeatTypeAsync();
            return Ok(response);
        }

        [HttpGet("{typeId:int}")]
        public async Task<IActionResult> GetSeatTypeByIdAsync(int typeId)
        {
            var response = await _seatTypeService.GetSeatTypeByIdAsync(typeId);
            if (response == null)
            {
                return NotFound(new ParentResponse { Message = Constants.MESSAGE_NOT_FOUND });
            }

            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public async Task<IActionResult> UpdateSeatType(SeatTypeUpdateRequest request)
        {
            var response = await _seatTypeService.UpdateSeatTypeAsync(request);
            if (response == null)
            {
                return BadRequest(new ParentResponse { Message = Constants.MESSAGE_OPERATION_FAILED});
            }

            return Ok(response);
        }
    }
}
