using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Dapper;
using Term7MovieCore.Data.Options;
using Microsoft.Extensions.Options;

namespace Term7MovieRepository.Repositories.Implement
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {

        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;
        public RefreshTokenRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption;
        }

        public async Task<RefreshToken> GetRefreshTokenByJtiAsync(string jti)
        {
            RefreshToken refreshToken = null;
            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql = 
                    " SELECT Id, Value, Jti, ExpiredDate, UserId, IsRevoked " +
                    " FROM RefreshTokens " +
                    " WHERE Jti = @jti ";
                var param = new { jti };
                refreshToken = await con.QueryFirstOrDefaultAsync<RefreshToken>(sql, param: param);
            }
            return refreshToken;
        }
        public async Task CreateRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
        }
        public long DeleteExpiredRefreshToken()
        {
            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    " DELETE FROM RefreshTokens " +
                    " WHERE ExpiredDate < GETUTCDATE() ";

                return con.Execute(sql);
            }
        }
        public async Task RevokeRefreshTokenAsync(string jti)
        {
            await Task.CompletedTask;
        }

    }
}
