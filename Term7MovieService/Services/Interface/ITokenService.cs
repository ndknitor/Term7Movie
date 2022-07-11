using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;

namespace Term7MovieService.Services.Interface
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user, string roleName, Guid jti);
        Task<string> GenerateRefreshTokenAsync(User user, Guid jti);

        Task<ExchangeTokenResponse> ExchangeNewAccessTokenAsync(ExchangeTokenRequest request);
    }
}
