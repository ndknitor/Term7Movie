using Dapper;
using Microsoft.Data.SqlClient;
using Term7MovieCore.Data.Collections;
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

        public async Task<PagingList<TransactionHistory>> GetAllTransactionHistory(ParentFilterRequest request)
        {
            PagingList<TransactionHistory> pagingList = new PagingList<TransactionHistory>();

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int fetch = request.PageSize;
                int offset = request.PageSize * (request.Page - 1);
                string sql = @" SELECT Id, UserId, TicketId, TicketPrice, TheaterId, TheaterName, PurchasedDate, MovieId, TransactionId
                                FROM TransactionHistories
                                ORDER BY Id DESC 
                                OFFSET @offset ROWS
                                FETCH NEXT @fetch ROWS ONLY ; ";

                string count = @" SELECT COUNT(Id) FROM TransactionHistories ";

                object param = new { fetch, offset };

                var multiQ = await con.QueryMultipleAsync(sql + count, param);

                var list = await multiQ.ReadAsync<TransactionHistory>();

                long total = await multiQ.ReadFirstOrDefaultAsync<long>();

                pagingList = new PagingList<TransactionHistory>(request.PageSize, request.Page, list, total);
            }

            return pagingList;
        }

        public async Task<PagingList<TransactionHistory>> GetAllTransactionHistoryByCustomerId(ParentFilterRequest request, long customerId)
        {
            PagingList<TransactionHistory> pagingList = new PagingList<TransactionHistory>();

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int fetch = request.PageSize;
                int offset = request.PageSize * (request.Page - 1 );
                string sql = @" SELECT Id, UserId, TicketId, TicketPrice, TheaterId, TheaterName, PurchasedDate, MovieId, TransactionId
                                FROM TransactionHistories
                                WHERE UserId = @customerId
                                ORDER BY Id DESC 
                                OFFSET @offset ROWS
                                FETCH NEXT @fetch ROWS ONLY ; ";

                string count = @" SELECT COUNT(Id) 
                                FROM TransactionHistories
                                WHERE UserId = @customerId ";

                object param = new { customerId, fetch, offset};

                var multiQ = await con.QueryMultipleAsync(sql + count, param);

                var list = await multiQ.ReadAsync<TransactionHistory>();

                long total = await multiQ.ReadFirstOrDefaultAsync<long>();

                pagingList = new PagingList<TransactionHistory>(request.PageSize, request.Page, list, total);

            }

            return pagingList;
        }
        public async Task<PagingList<TransactionHistory>> GetAllTransactionHistoryByCompanyId(ParentFilterRequest request, long managerId)
        {
            PagingList<TransactionHistory> pagingList = new PagingList<TransactionHistory>();

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int fetch = request.PageSize;
                int offset = request.PageSize * (request.Page - 1);

                string sql = @" SELECT trh.Id, trh.UserId, trh.TicketId, trh.TicketPrice, trh.TheaterId, trh.TheaterName, trh.PurchasedDate, trh.MovieId, trh.TransactionId
                                FROM TransactionHistories trh JOIN Theaters th ON trh.TheaterId = th.Id
                                WHERE th.ManagerId = @managerId
                                ORDER BY Id DESC 
                                OFFSET @offset ROWS
                                FETCH NEXT @fetch ROWS ONLY ; ";

                string count = @" SELECT COUNT(trh.Id) 
                                FROM TransactionHistories trh JOIN Theaters th ON trh.TheaterId = th.Id
                                WHERE th.ManagerId = @managerId ";

                object param = new { fetch, offset, managerId };

                var multiQ = await con.QueryMultipleAsync(sql + count, param);

                var list = await multiQ.ReadAsync<TransactionHistory>();

                long total = await multiQ.ReadFirstOrDefaultAsync<long>();

                pagingList = new PagingList<TransactionHistory>(request.PageSize, request.Page, list, total);
            }

            return pagingList;
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
