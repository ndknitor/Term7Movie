using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Extensions;
using Term7MovieCore.Data.Request;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/transaction-histories")]
    [ApiController]
    public class TransactionHistoryController : ControllerBase
    {
        private readonly ITransactionHistoryService _transactionHistoryService;

        public TransactionHistoryController(ITransactionHistoryService transactionHistoryService)
        {
            _transactionHistoryService = transactionHistoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListTransactionHistoryAsync([FromQuery] ParentFilterRequest request)
        {
            string role = User.Claims.FindFirstValue(Constants.JWT_CLAIM_ROLE);
            long userId = Convert.ToInt64(User.Claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));

            var response = await _transactionHistoryService.GetTransactionListHistoryAynsc(request, role, userId);

            return Ok(response);

        }
    }
}
