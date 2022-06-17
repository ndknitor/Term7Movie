using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Enum;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IUserService _userService;
        public RoleController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        [HttpPut]
        public async Task<IActionResult> ChangeRoleAsync(RoleUpdateRequest request)
        {
            var response = await _userService.UpdateUserRole(request);

            return Ok(response);
        } 
    }
}
