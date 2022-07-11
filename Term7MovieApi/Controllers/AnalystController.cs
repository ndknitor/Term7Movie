using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Extensions;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/analyst")]
    [ApiController]
    public class AnalystController : ControllerBase
    {
        private readonly IAnalystService _anaService;
        //private readonly ILogger<AnalystController> _logger;

        public AnalystController(IAnalystService anaService/*, ILogger<AnalystController> logger*/)
        {
            _anaService = anaService;
            //_logger = logger;
        }

        [HttpGet("dashboard")]
        [Authorize(Roles = Constants.ROLE_MANAGER)]
        public async Task<IActionResult> QuickDashboard(int companyid)
        {
            long? managerId = Convert.ToInt64(User.Claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));
            //gonna check manager later
            var result = await _anaService.GetQuickAnalystDashboardForManager(companyid, managerId);
            //_logger.LogInformation("You cost: " + slow.ElapsedMilliseconds);
            return Ok(result);
        }

        [HttpGet("dashboard/admin")]
        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public async Task<IActionResult> QuickDashboardForAdmin()
        {
            var result = await _anaService.GetQuickAnalystDashboardForAdmin();
            return Ok(result);
        }

        [HttpGet("yearly-income/admin")]
        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public async Task<IActionResult> YearlyIncome(int year)
        {
            var result = await _anaService.GetYearlyIncomeForAdmin(year);
            return Ok(result);
        }

        [HttpGet("yearly-income/manager")]
        [Authorize(Roles = Constants.ROLE_MANAGER)]
        public async Task<IActionResult> YearlyIncome(int year, int companyid)
        {
            long? managerId = Convert.ToInt64(User.Claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));
            var result = await _anaService.GetYearlyIncomeForManager(companyid, year, managerId);
            return Ok(result);
        }
    }
}
