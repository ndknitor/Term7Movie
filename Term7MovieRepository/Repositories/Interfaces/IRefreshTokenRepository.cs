using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {

        Task<RefreshToken> GetRefreshTokenByJtiAsync(string jti);
        Task CreateRefreshTokenAsync(RefreshToken refreshToken);
        Task DeleteRefreshTokenAsync(long id);
        Task RevokeRefreshTokenAsync(string jti);

    }
}
