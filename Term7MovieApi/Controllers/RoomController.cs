using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/room")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Authorize(Roles = $"{Constants.ROLE_ADMIN}, {Constants.ROLE_MANAGER}")]
        [HttpPost("create-room")]
        public async Task<IActionResult> CreateRoom(RoomCreateRequest request)
        {
            var response = await _roomService.CreateRoom(request);

            if(response == null)
            {
                return BadRequest(new ParentResponse { Message = Constants.MESSAGE_OPERATION_FAILED });
            }
            return Ok();
        }

        [Authorize]
        [HttpGet("get-room/{id:int}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var response = await _roomService.GetRoomDetail(id);

            if (response == null)
            {
                return BadRequest(new ParentResponse() { Message = Constants.MESSAGE_NOT_FOUND });
            }

            return Ok(response);
        }


        [Authorize]
        [HttpGet("get-theater-rooms/{theaterId:int}")]
        public async Task<IActionResult> GetRoomsByTheaterId(int theaterId)
        {
            var response = await _roomService.GetRoomsByTheaterId(theaterId);

            if (response == null)
            {
                return BadRequest(new ParentResponse { Message = Constants.MESSAGE_NOT_FOUND });
            }
            return Ok();
        }
    }
}
