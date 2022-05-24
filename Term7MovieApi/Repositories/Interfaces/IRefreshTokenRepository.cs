using Term7MovieApi.Entities;

namespace Term7MovieApi.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {

        RefreshToken GetRefreshTokenByJti(Guid Jti);
        int CreateRefreshToken(RefreshToken refreshToken);
        int DeleteRefreshToken(long id);
        int RevokeRefreshToken(Guid Jti);

    }
}
