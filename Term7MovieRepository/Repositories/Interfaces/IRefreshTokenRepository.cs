using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {

        Task<RefreshToken> GetRefreshTokenByJtiAsync(string jti);
        Task CreateRefreshTokenAsync(RefreshToken refreshToken);
        long DeleteExpiredRefreshToken();
        Task RevokeRefreshTokenAsync(string jti);

    }
}
