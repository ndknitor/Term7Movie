using Dapper;
using Microsoft.Data.SqlClient;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class TransactionHistoryRepository : ITransactionHistoryRepository
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;
        public TransactionHistoryRepository(AppDbContext context, ConnectionOption connection)
        {
            _context = context;
            _connectionOption = connection;
        }

        public async Task<IEnumerable<TransactionHistory>> GetAllTransactionHistory(ParentFilterRequest request)
        {
            IEnumerable<TransactionHistory> list = new List<TransactionHistory>();

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int fetch = request.PageSize;
                int offset = request.PageSize * (request.Page - 1);
                string sql = @" SELECT Id, UserId, TicketId, TicketPrice, TheaterId, TheaterName, PurchasedDate, MovieId, TransactionId
                                FROM TransactionHistories
                                ORDER BY Id DESC 
                                OFFSET @offset ROWS
                                FETCH NEXT @fetch ROWS ONLY ";
                object param = new { fetch, offset };
                list = await con.QueryAsync<TransactionHistory>(sql, param);
            }

            return list;
        }

        public async Task<IEnumerable<TransactionHistory>> GetAllTransactionHistoryByCustomerId(ParentFilterRequest request, long customerId)
        {
            IEnumerable<TransactionHistory> list = new List<TransactionHistory>();

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int fetch = request.PageSize;
                int offset = request.PageSize * (request.Page - 1 );
                string sql = @" SELECT Id, UserId, TicketId, TicketPrice, TheaterId, TheaterName, PurchasedDate, MovieId, TransactionId
                                FROM TransactionHistories
                                WHERE UserId = @customerId
                                ORDER BY Id DESC 
                                OFFSET @offset ROWS
                                FETCH NEXT @fetch ROWS ONLY ";
                object param = new { customerId, fetch, offset};
                list = await con.QueryAsync<TransactionHistory>(sql, param);
            }

            return list;
        }
        public async Task<IEnumerable<TransactionHistory>> GetAllTransactionHistoryByCompanyId(ParentFilterRequest request, long managerId)
        {
            IEnumerable<TransactionHistory> list = new List<TransactionHistory>();

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int fetch = request.PageSize;
                int offset = request.PageSize * (request.Page - 1);
                string sql = @" SELECT Id, UserId, TicketId, TicketPrice, TheaterId, TheaterName, PurchasedDate, MovieId, TransactionId
                                FROM TransactionHistories trh JOIN Theaters th ON trh.TheaterId = th.Id
                                WHERE th.ManagerId = @managerId
                                ORDER BY Id DESC 
                                OFFSET @offset ROWS
                                FETCH NEXT @fetch ROWS ONLY ";
                object param = new { fetch, offset };
                list = await con.QueryAsync<TransactionHistory>(sql, param);
            }

            return list;
        }
        public TransactionHistory GetTransactionHistoryById(long id)
        {
            TransactionHistory history = null;
            return history;
        }
        public async Task CreateTransactionHistory(IEnumerable<long> idList)
        {
            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    " INSERT INTO TransactionHistories (UserId, TicketId, TicketPrice, TheaterId, TheaterName, PurchasedDate, MovieId, TransactionId) " +
                    " SELECT tr.CustomerId, tk.Id, tk.SellingPrice, th.Id, th.Name, tr.PurchasedDate, sh.MovieId, tr.Id " +
                    " FROM Tickets tk JOIN Showtimes sh ON tk.ShowTimeId = sh.Id " +
                    "   JOIN Theaters th ON sh.TheaterId = th.Id " +
                    "   JOIN Transactions tr ON tk.TransactionId = tr.Id " +
                    " WHERE tk.Id IN @idList ";
                object param = new { idList };

                await con.ExecuteAsync(sql, param);
            }
        }
    }
}
