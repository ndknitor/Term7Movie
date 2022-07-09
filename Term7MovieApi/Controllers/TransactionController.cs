using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Extensions;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Extensions;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllTransactionAsync([FromQuery] TransactionFilterRequest request)
        {
            long userId = Convert.ToInt64(User.Claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));
            string role = User.Claims.FindFirstValue(Constants.JWT_CLAIM_ROLE);

            var response = await _transactionService.GetAllTransactionAsync(request, userId, role);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("{transactionId:Guid}")]
        public async Task<IActionResult> GetAllTransactionAsync(Guid transactionId)
        {

            var response = await _transactionService.GetTransactionByIdAsync(transactionId);

            return Ok(response);
        }

        [Authorize(Roles = Constants.ROLE_CUSTOMER)] // policy check if ticket is available
        [HttpPost]
        public IActionResult CreateTransactionAsync([FromBody] TransactionCreateRequest request)
        {
            IEnumerable<Claim> claims = User.Claims;
            UserDTO user = new UserDTO
            {
                Id = Convert.ToInt64(claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID)),
                FullName = claims.FindFirstValue(Constants.JWT_CLAIM_NAME),
                Email = claims.FindFirstValue(Constants.JWT_CLAIM_EMAIL)
            };

            var response = _transactionService.CreateTransaction(request, user);

            return Ok(response);
        }

        [Authorize(Roles = Constants.ROLE_CUSTOMER)] // policy check if ticket is available
        [HttpGet("complete-payment")]
        public async Task<IActionResult> CompletePayment([Required][FromQuery] Guid transactionId)
        {
            long userId = Convert.ToInt64(User.Claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));
            var response = await _transactionService.CheckPaymentStatus(transactionId, userId);

            return Ok(response);
        }

        [HttpPost("call-back")]
        private async Task<IActionResult> MomoIPNCallBack(MomoIPNRequest request)
        {
            await _transactionService.ProcessPaymentAsync(request);
            return NoContent();
        }

        [HttpGet("status-code")]
        public IActionResult GetStatusCodes()
        {
            return Ok(new ParentResultResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                Result = "[{\"Id\":1,\"Name\":\"Failed\"},{\"Id\":2,\"Name\":\"Successful\"},{\"Id\":3,\"Name\":\"Pending\"},{\"Id\":4,\"Name\":\"Cancelled\"}]".ToObject<IEnumerable<TransactionStatusDto>>()
            });
        }
    }
}
