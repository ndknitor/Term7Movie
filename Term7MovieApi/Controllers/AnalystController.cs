using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Term7MovieCore.Data;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/analyst")]
    [ApiController]
    public class AnalystController : ControllerBase
    {
        private readonly IAnalystService _anaService;
        private readonly ILogger<AnalystController> _logger;

        public AnalystController(IAnalystService anaService, ILogger<AnalystController> logger)
        {
            _anaService = anaService;
            _logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles = Constants.ROLE_MANAGER)]
        public async Task<IActionResult> QuickDashboard(int companyid)
        {
            //gonna check manager later
            Stopwatch slow = Stopwatch.StartNew();
            var result = await _anaService.GetQuickAnalystForDashboard(companyid);
            _logger.LogInformation("You cost: " + slow.ElapsedMilliseconds);
            return Ok(result);
        }
    }
}
