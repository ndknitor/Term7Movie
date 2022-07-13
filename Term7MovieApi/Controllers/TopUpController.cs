using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Extensions;
using Term7MovieCore.Data.Request;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/top-up")]
    [ApiController]
    public class TopUpController : ControllerBase
    {
        private readonly ITopUpService topUpService;

        public TopUpController(ITopUpService topUpService)
        {
            this.topUpService = topUpService;
        }

        [HttpGet]
        [Authorize(Roles = Constants.ROLE_CUSTOMER + "," + Constants.ROLE_ADMIN)]
        public async Task<IActionResult> GetTopUpHistory([FromQuery] TopUpHistoryFilterRequest request)
        {
            long userId = Convert.ToInt64(User.Claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));
            string role = User.Claims.FindFirstValue(Constants.JWT_CLAIM_ROLE);

            var response = await topUpService.GetAllTopUpHistoryAsync(request, userId, role);

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = Constants.ROLE_CUSTOMER)]
        public async Task<IActionResult> TopUpAsync([FromBody] TopUpRequest request)
        {
            long userId = Convert.ToInt64(User.Claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));

            var response = await topUpService.TopUpAsync(request, userId);

            return Ok(response);
        }
    }
}
