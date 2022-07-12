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
        [Authorize(Roles = Constants.ROLE_MANAGER + "," +  Constants.ROLE_ADMIN)]
        public async Task<IActionResult> QuickDashboard()
        {
            long? managerId = Convert.ToInt64(User.Claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));
            string role = User.Claims.FindFirstValue(Constants.JWT_CLAIM_ROLE);
            switch(role)
            {
                case Constants.ROLE_MANAGER:
                    return await QuickDashboardForManager(managerId);
                case Constants.ROLE_ADMIN:
                    return await QuickDashboardForAdmin();
                default:
                    return Forbid();
            }
        }

        [HttpGet("yearly-income")]
        [Authorize(Roles = Constants.ROLE_ADMIN + "," + Constants.ROLE_MANAGER)]
        public async Task<IActionResult> YearlyIncome(int year)
        {
            string role = User.Claims.FindFirstValue(Constants.JWT_CLAIM_ROLE);
            long? managerId = Convert.ToInt64(User.Claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));
            switch(role)
            {
                case Constants.ROLE_MANAGER:
                    return await YearlyIncomeForManager(managerId, year);
                case Constants.ROLE_ADMIN:
                    return await YearlyIncomeForAdmin(year);
                default:
                    return Forbid();
            }
        }


        private async Task<IActionResult> QuickDashboardForAdmin()
        {
            var result = await _anaService.GetQuickAnalystDashboardForAdmin();
            return Ok(result);
        }

        private async Task<IActionResult> QuickDashboardForManager(long? managerid)
        {
            var result = await _anaService.GetQuickAnalystDashboardForManager(managerid);
            return Ok(result);
        }


        private async Task<IActionResult> YearlyIncomeForManager(long? managerid, int year)
        {
            var result = await _anaService.GetYearlyIncomeForManager(year, managerid);
            return Ok(result);
        }

        private async Task<IActionResult> YearlyIncomeForAdmin(int year)
        {
            var result = await _anaService.GetYearlyIncomeForAdmin(year);
            return Ok(result);
        }
    }
}
