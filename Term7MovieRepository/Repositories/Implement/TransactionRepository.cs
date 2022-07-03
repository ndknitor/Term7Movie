using Dapper;
using Microsoft.Data.SqlClient;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption connectionOption;

        private const string FILTER_BY_ROLE = "Role";
        private const string FILTER_BY_ID = "Id";
        private const string FILTER_BY_STATUS = "Status";

        public TransactionRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            this.connectionOption = connectionOption;
        }
        public void CreateTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
        }
        public async Task UpdateTransaction(Guid id, int statusId, int momoStatus)
        {
            Transaction dbTransaction = await _context.Transactions.FindAsync(id);

            if (dbTransaction == null) throw new DbNotFoundException();

            dbTransaction.StatusId = statusId;
            dbTransaction.MomoResultCode = momoStatus;
        }

        // Background service => update transaction where staus = pending to cancelled when ValidUntil time due

        public async Task<PagingList<TransactionDto>> GetAllTransactionAsync(TransactionFilterRequest request, long userId, string role) 
        {
            PagingList<TransactionDto> list = null;

            using(SqlConnection con = new SqlConnection(connectionOption.FCinemaConnection))
            {
                int fetch = request.PageSize;
                int offset = (request.Page - 1) * request.PageSize;

                string sql =
                    @" SELECT trn.Id, trn.CustomerId, trn.TheaterId, th.Name 'TheaterName', trn.PurchasedDate, trn.Total, trn.QRCodeUrl, trn.ValidUntil, trn.MomoResultCode, trn.StatusId, trns.Name 'StatusName' 
                       FROM Transactions trn JOIN TransactionStatuses trns ON trn.StatusId = trns.Id 
                            JOIN Theaters th ON trn.TheaterId = th.Id
                       WHERE 1=1 " +

                       GetAdditionTransactionFilter(request, userId, role, FILTER_BY_ROLE) +
                       GetAdditionTransactionFilter(request, userId, role, FILTER_BY_ID) +
                       GetAdditionTransactionFilter(request, userId, role, FILTER_BY_STATUS) + 
                       
                    @" ORDER BY  trn.PurchasedDate DESC 
                       OFFSET @offset ROWS
                       FETCH NEXT @fetch ROWS ONLY ; ";

                string count =
                    @" SELECT COUNT(*) 
                       FROM Transactions trn JOIN TransactionStatuses trns ON trn.StatusId = trns.Id 
                            JOIN Theaters th ON trn.TheaterId = th.Id " +

                       GetAdditionTransactionFilter(request, userId, role, FILTER_BY_ROLE) +
                       GetAdditionTransactionFilter(request, userId, role, FILTER_BY_ID) +
                       GetAdditionTransactionFilter(request, userId, role, FILTER_BY_STATUS) +
                    " ; ";
                object param = new { request.StatusId, userId, request.TransactionId, offset, fetch };

                var multiQ = await con.QueryMultipleAsync(sql + count, param);

                IEnumerable<TransactionDto> transactions = await multiQ.ReadAsync<TransactionDto>();
                long total = await multiQ.ReadFirstOrDefaultAsync<long>();

                list = new PagingList<TransactionDto>(request.PageSize, request.Page, transactions, total);
            }

            return list;
        }

        private string GetAdditionTransactionFilter(TransactionFilterRequest request, long userId, string role, string filter)
        {
            string query = "";

            switch(filter)
            {
                case FILTER_BY_ID:
                    if (request.TransactionId != null)
                    {
                        query = " AND trn.Id = @TransactionId ";
                    }
                    break;
                case FILTER_BY_ROLE:

                    if (role == Constants.ROLE_CUSTOMER)
                    {
                        return " AND trn.CustomerId = @userId ";
                    }

                    if (role == Constants.ROLE_MANAGER)
                    {
                        return " AND th.ManagerId = @userId ";
                    }

                    break;
                case FILTER_BY_STATUS:
                    if (request.StatusId != null)
                    {
                        query = " AND trns.Id = @StatusId ";
                    }
                    break;
            }

            return query;
        }

        public async Task<TransactionDto> GetTransactionByIdAsync(Guid transactionId)
        {

            using (SqlConnection con = new SqlConnection(connectionOption.FCinemaConnection))
            {

                string sql =
                    @" SELECT trn.Id, trn.CustomerId, trn.TheaterId, th.Name 'TheaterName', trn.PurchasedDate, trn.Total, trn.QRCodeUrl, trn.ValidUntil, trn.MomoResultCode, trn.StatusId, trns.Name 'StatusName' 
                              , u.Id, u.FullName, u.Email
                       FROM Transactions trn JOIN TransactionStatuses trns ON trn.StatusId = trns.Id 
                            JOIN Theaters th ON trn.TheaterId = th.Id
                            JOIN Users u ON trn.CustomerId = u.Id
                       WHERE trn.Id = @transactionId ; ";

                string queryTicket =
                    @" SELECT t.Id, t.ShowtimeId, t.ShowStartTime, 
                              t.ReceivePrice, t.SellingPrice, t.StatusId, ts.Name 'StatusName',
                              t.LockedTime, t.TransactionId, t.ShowtimeTicketTypeId,
                              t.SeatId, s.Id, s.Name, s.ColumnPos, s.RowPos, s.SeatTypeId, st.Id, st.Name,  
                              tt.Id, tt.Name, tt.CompanyId   
                       FROM Tickets t JOIN Seats s ON t.SeatId = s.Id
                            JOIN SeatTypes st ON s.SeatTypeId = st.Id
                            JOIN TicketStatuses ts ON t.StatusId = ts.Id 
                            JOIN ShowtimeTicketTypes shtt ON shtt.Id = t.ShowtimeTicketTypeId 
                            JOIN TicketTypes tt ON shtt.TicketTypeId = tt.Id
                       WHERE t.TransactionId = @transactionId ";

                    object param = new { transactionId };

                var multiQ = await con.QueryMultipleAsync(sql + queryTicket, param);

                TransactionDto transaction = multiQ.Read<TransactionDto, UserDTO, TransactionDto>(
                    (trn, u) =>
                    {
                        trn.Customer = u;
                        return trn;
                    }, splitOn: "Id").FirstOrDefault();

                if (transaction == null) throw new DbNotFoundException();

                transaction.Tickets = multiQ.Read<TicketDto, SeatDto, SeatTypeDto, TicketTypeDto, TicketDto>(
                    (t, s, st, tt) =>
                    {
                        s.SeatType = st;
                        t.Seat = s;
                        t.TicketType = tt;
                        return t;
                    });

                return transaction;
            }
        }
    }
}
