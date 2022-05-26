using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {

        RefreshToken GetRefreshTokenByJti(Guid Jti);
        int CreateRefreshToken(RefreshToken refreshToken);
        int DeleteRefreshToken(long id);
        int RevokeRefreshToken(Guid Jti);

    }
}
