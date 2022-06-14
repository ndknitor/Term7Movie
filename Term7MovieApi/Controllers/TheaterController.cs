using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Extensions;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/theaters")]
    [ApiController]
    public class TheaterController : ControllerBase
    {
        private readonly ILogger<TheaterController> _logger;
        private readonly ITheaterService _theaterService;

        public TheaterController(ILogger<TheaterController> logger, ITheaterService theaterService)
        {
            _theaterService = theaterService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTheatersAsync([FromQuery] TheaterFilterRequest request)
        {
            var response = await _theaterService.GetTheatersAsync(request);
            return Ok(response);
        }

        [HttpGet("{theaterId:int}")]
        [Authorize]
        public async Task<IActionResult> GetTheatersAsync(int theaterId)
        {
            var response = await _theaterService.GetTheaterByIdAsync(theaterId);

            if (response == null)
            {
                return NotFound(new ParentResponse { Message = Constants.MESSAGE_NOT_FOUND });
            }

            return Ok(response);
        }

        [HttpGet("names")]
        [Authorize]
        public async Task<IActionResult> GetTheaterByCompanyId(int? companyId)
        {
            try
            {
                string role = User.Claims.FindFirstValue(Constants.JWT_CLAIM_ROLE);
                long? managerid = null;
                if(Constants.ROLE_MANAGER.Equals(role))
                {
                    managerid = Convert.ToInt64(User.Claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));
                }
                _logger.LogInformation(role + "_" + managerid + "_" + companyId);
                if (companyId == null)
                    throw new Exception("400");
                _logger.LogInformation("Checkmate");
                var result = await _theaterService.GetTheaterNamesFromCompany(companyId.Value, managerid);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                if (ex.Message == "400")
                    return BadRequest(new ParentResponse { Message = "Logic went wrong or access deny." });
                return BadRequest(new ParentResponse { Message = "Chụp hình gửi Nam Trần nha huhu. " + ex.Message });
            }
        }


        [HttpPost]
        [Authorize(Roles = Constants.ROLE_MANAGER)]
        public async Task<IActionResult> CreateTheaterAsync(TheaterCreateRequest request)
        {
            long managerId = Convert.ToInt64(User.Claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));

            var response = await _theaterService.CreateTheaterAsync(request, managerId);

            if (response == null)
            {
                return BadRequest(new ParentResponse { Message = Constants.MESSAGE_OPERATION_FAILED });
            }

            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = Constants.ROLE_MANAGER)]
        public async Task<IActionResult> UpdateTheaterAsync(TheaterUpdateRequest request)
        {
            var response = await _theaterService.UpdateTheaterAsync(request);

            if (response == null)
            {
                return BadRequest(new ParentResponse { Message = Constants.MESSAGE_OPERATION_FAILED });
            }

            return Ok(response);
        }

        [HttpDelete("{theaterId:int}")]
        [Authorize(Roles = Constants.ROLE_MANAGER)]
        public async Task<IActionResult> DeleteTheaterAsync(int theaterId)
        {
            var response = await _theaterService.DeleteTheaterAsync(theaterId);

            if (response == null)
            {
                return BadRequest(new ParentResponse { Message = Constants.MESSAGE_OPERATION_FAILED });
            }

            return Ok(response);
        }

        [HttpGet("location")]
        private async Task<IActionResult> GetLocationAsync([FromQuery][Required] string address)
        {
            var response = await _theaterService.GetLocationByAddressAsync(address);

            return Ok(response);
        }
    }
}
