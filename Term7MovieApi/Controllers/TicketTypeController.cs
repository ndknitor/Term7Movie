using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Extensions;
using Term7MovieCore.Data.Request;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/ticket-types")]
    [ApiController]
    [Authorize(Roles = Constants.ROLE_MANAGER)]
    public class TicketTypeController : ControllerBase
    {
        private readonly ITicketTypeService _ticketTypeService;

        public TicketTypeController(ITicketTypeService ticketTypeService)
        {
            _ticketTypeService = ticketTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTicketTypeAsync()
        {
            long managerId = Convert.ToInt64(User.Claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));

            var response = await _ticketTypeService.GetAllTicketTypeAsync(managerId);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicketTypeAsync(TicketTypeCreateRequest request)
        {
            long managerId = Convert.ToInt64(User.Claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));

            var response = await _ticketTypeService.CreateTicketType(request, managerId);

            return Ok(response);
        }
    }
}
