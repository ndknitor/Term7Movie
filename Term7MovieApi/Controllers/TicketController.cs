using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        //private readonly IUnitOfWork _unitOfWork;

        public TicketController(ILogger<MovieController> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        [HttpGet("{id:int}")]
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
        public async Task<IActionResult> CreateTicket(TicketCreateRequest[] request)
        {
            try
            {
                if (request.Length > 1)
                {
                    var response = await _ticketService.CreateALotOfTicket(request);
                    return Ok(response);
                }
                else if (request.Length == 1)
                {
                    var response = await _ticketService.CreateTicket(request.First());
                    return Ok(response);
                }
                return BadRequest(new ParentResponse { Message = "Request gone wrong" });
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(new ParentResponse { Message = "Send this to Nam Tran: " + ex.Message });
            }
        }
    }
}
