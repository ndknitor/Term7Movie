using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authServices;

        public AuthController(IAuthService authService)
        {
            _authServices = authService;
        }

        [NonAuthorized]
        [HttpPost("google-sign-in")]
        public async Task<IActionResult> SignIn(SignInRequest request)
        {
            AuthResponse response = await _authServices.GoogleSignInAsync(request.IdToken);

            if(response == null)
            {
                return BadRequest(new ParentResponse { Message = Constants.MESSAGE_INVALID_ACCOUNT });
            }

            return Ok(response);
        }

        [NonAuthorized]
        [HttpPost("token")]
        public async Task<IActionResult> ExchangeToken(ExchangeTokenRequest request)
        {
            ExchangeTokenResponse response = await _authServices.ExchangeTokenAsync(request);

            if (response.AccessToken == null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
