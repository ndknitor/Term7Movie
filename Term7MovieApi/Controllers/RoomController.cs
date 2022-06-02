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

        [Authorize]
        [HttpGet("room/{id:int}")]
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
        [HttpGet("theater-rooms/{theaterId:int}")]
        public async Task<IActionResult> GetRoomsByTheaterId(int theaterId)
        {
            var response = await _roomService.GetRoomsByTheaterId(theaterId);

            if (response == null)
            {
                return BadRequest(new ParentResponse { Message = Constants.MESSAGE_NOT_FOUND });
            }
            return Ok(response);
        }

        [Authorize(Roles = $"{Constants.ROLE_ADMIN}, {Constants.ROLE_MANAGER}")]
        [HttpPost("room")]
        public async Task<IActionResult> CreateRoom(RoomCreateRequest request)
        {
            var response = await _roomService.CreateRoom(request);

            if (response == null)
            {
                return BadRequest(new ParentResponse { Message = Constants.MESSAGE_OPERATION_FAILED });
            }
            return Ok(response);
        }

        [Authorize(Roles = $"{Constants.ROLE_ADMIN}, {Constants.ROLE_MANAGER}")]
        [HttpPut("room")]
        public async Task<IActionResult> UpdateRoom(RoomUpdateRequest request)
        {
            var response = await _roomService.UpdateRoom(request);

            if (response == null)
            {
                return BadRequest(new ParentResponse { Message = Constants.MESSAGE_NOT_FOUND });
            }
            return Ok(response);
        }

        [Authorize(Roles = $"{Constants.ROLE_ADMIN}, {Constants.ROLE_MANAGER}")]
        [HttpDelete("room/{roomId:int}")]
        public async Task<IActionResult> DeleteRoom(int roomId)
        {
            var response = await _roomService.DeleteRoom(roomId);

            if (response == null)
            {
                return BadRequest(new ParentResponse { Message = Constants.MESSAGE_NOT_FOUND });
            }
            return Ok(response);
        }
    }
}
