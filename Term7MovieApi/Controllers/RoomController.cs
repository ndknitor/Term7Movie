﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Authorize]
        [HttpGet("{id:int}")]
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
        [HttpGet]
        public async Task<IActionResult> GetRoomsByTheaterId([FromQuery] RoomFilterRequest request)
        {
            var response = await _roomService.GetRoomsByTheaterId(request);

            return Ok(response);
        }

        [Authorize(Roles = Constants.ROLE_MANAGER, Policy = Constants.POLICY_CREATE_ROOM_SAME_THEATER)]
        [HttpPost]
        public async Task<IActionResult> CreateRoom(RoomCreateRequest request)
        {
            var response = await _roomService.CreateRoom(request);

            if (response == null)
            {
                return BadRequest(new ParentResponse { Message = Constants.MESSAGE_OPERATION_FAILED });
            }
            return Ok(response);
        }

        [Authorize(Roles = Constants.ROLE_MANAGER, Policy = Constants.POLICY_UPDATE_ROOM_SAME_THEATER)]
        [HttpPut]
        public async Task<IActionResult> UpdateRoom(RoomUpdateRequest request)
        {
            var response = await _roomService.UpdateRoom(request);

            if (response == null)
            {
                return BadRequest(new ParentResponse { Message = Constants.MESSAGE_NOT_FOUND });
            }
            return Ok(response);
        }

        [Authorize(Roles = Constants.ROLE_MANAGER, Policy = Constants.POLICY_DELETE_ROOM_SAME_THEATER)]
        [HttpDelete("{roomId:int}")]
        public async Task<IActionResult> DeleteRoom(int roomId)
        {
            var response = await _roomService.DeleteRoom(roomId);

            if (response == null)
            {
                return BadRequest(new ParentResponse { Message = Constants.MESSAGE_NOT_FOUND });
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("lossless/{theaterid:int}")]
        public async Task<IActionResult> GetRoomNumberForATheater(int theaterid)
        {
            try
            {
                var response = await _roomService.GetRoomNumberFromTheater(theaterid);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(new ParentResponse { Message = "Chụp hình gửi Nam Trần nha huhu. " + ex.Message });
            }
        }
    }
}
