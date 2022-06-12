using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Extensions;
using Term7MovieCore.Data.Request;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        //[Authorize(Roles = Constants.ROLE_CUSTOMER)]
        [HttpPost]
        public async Task<IActionResult> CreateTransactionAsync(TransactionCreateRequest request)
        {
            IEnumerable<Claim> claims = User.Claims;
            UserDTO user = new UserDTO
            {
                Id = Convert.ToInt64(claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID)),
                FullName = claims.FindFirstValue(Constants.JWT_CLAIM_NAME),
                Email = claims.FindFirstValue(Constants.JWT_CLAIM_EMAIL)
            };

            var response = await _transactionService.CreateTransactionAsync(request, user);

            return Ok(response);
        }

        //[Authorize(Roles = Constants.ROLE_CUSTOMER)]
        [HttpGet("payment-complete")]
        public IActionResult PaymentComplete()
        {
            return Ok("Payment Complete");
        }

        [HttpPost("call-back")]
        public async Task<IActionResult> MomoIPNCallBack(MomoIPNRequest request)
        {
            await _transactionService.ProcessPaymentAsync(request);
            return NoContent();
        }
    }
}
