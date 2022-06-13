using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Extensions;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/companies")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [Authorize]
        [HttpGet("detail")]
        public async Task<IActionResult> GetCompanyDetail(int? companyId)
        {
            string role = User.Claims.FindFirstValue(Constants.JWT_CLAIM_ROLE);
            long? managerId = null;
            if (Constants.ROLE_MANAGER.Equals(role))
            {
                managerId = Convert.ToInt64(User.Claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));
            }

            var response = await _companyService.GetCompanyDetailAsync(companyId, managerId);

            return Ok(response);
        }
    }
}
