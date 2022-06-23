using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data;
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

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetTicketById(long id)
        {
            try
            {
                var response = await _ticketService.GetDetailOfATicket(id);
                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(new ParentResponse { Message = "Send this to Nam Tran: " });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTicketFromSomething(TicketRequest request)
        {
            try
            {
                var response = await _ticketService.GetTicketForSomething(request);
                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(new ParentResponse { Message = "Send this to Nam Tran: " });
            }
        }

        [HttpPost]
        [Authorize(Roles = Constants.ROLE_MANAGER, Policy = Constants.POLICY_MANAGER_CREATE_TICKET)]
        public async Task<IActionResult> CreateTicket([FromBody] TicketListCreateRequest request)
        {
            var response = await _ticketService.CreateTicketAsync(request);

            return Ok(response);
        }
    }
}
