﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        [Authorize(Roles = Constants.ROLE_MANAGER)]
        public async Task<IActionResult> QuickDashboard(int companyid)
        {
            try
            {
                long? managerId = Convert.ToInt64(User.Claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));
                //gonna check manager later
                var result = await _anaService.GetQuickAnalystForDashboard(companyid, managerId);
                //_logger.LogInformation("You cost: " + slow.ElapsedMilliseconds);
                return Ok(result);
            }
            catch(Exception ex)
            {
                if (ex.Message == "403")
                    return Forbid();
                throw new Exception(ex.Message); //will exceptionfilter catch this one?
            }
        }
    }
}
