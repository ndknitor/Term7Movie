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
    [Route("api/v1/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly ITicketService _ticketService;

        public TicketController(ILogger<MovieController> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        [HttpGet("{ticketId:long}")]
        [Authorize]
        public async Task<IActionResult> GetTicketById([FromQuery][Required] long showtimeId, long ticketId)
        {
            string role = User.Claims.FindFirstValue(Constants.JWT_CLAIM_ROLE);

            var response = await _ticketService.GetTicketDetail(ticketId, showtimeId, role);

            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTicketAsync([FromQuery] TicketFilterRequest request)
        {
            var response = await _ticketService.GetTicketAsync(request);

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = Constants.ROLE_MANAGER, Policy = Constants.POLICY_MANAGER_CREATE_TICKET)]
        public async Task<IActionResult> CreateTicket([FromBody] TicketListCreateRequest request)
        {
            var response = await _ticketService.CreateTicketAsync(request);

            return Ok(response);
        }

        [HttpPatch("lock")]
        [Authorize(Roles = Constants.ROLE_CUSTOMER)]
        public async Task<IActionResult> LockTicketAsync([FromBody] LockTicketRequest request)
        {
            var response = await _ticketService.LockTicketAsync(request);
            return Ok(response);
        }

        [HttpGet("sales")]
        public IActionResult SanSaleShopee()
        {//shortest code challenge lul
            return Ok(_ticketService.GetTicketOnSelling());
        }
    }
}
