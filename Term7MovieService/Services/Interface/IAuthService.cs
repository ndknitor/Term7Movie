using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface IAuthService
    {
        Task<AuthResponse> GoogleSignInAsync(string idToken);

        Task<ExchangeTokenResponse> ExchangeTokenAsync(ExchangeTokenRequest request);
    }
}
