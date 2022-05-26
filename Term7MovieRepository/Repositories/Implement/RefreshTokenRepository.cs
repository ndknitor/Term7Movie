using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {

        private AppDbContext _context;
        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public RefreshToken GetRefreshTokenByJti(Guid Jti)
        {
            RefreshToken refreshToken = null;
            return refreshToken;
        }
        public int CreateRefreshToken(RefreshToken refreshToken)
        {
            int count = 0;
            return count;
        }
        public int DeleteRefreshToken(long id)
        {
            int count = 0;
            return count;
        }
        public int RevokeRefreshToken(Guid Jti)
        {
            int count = 0;
            return count;
        }

    }
}
