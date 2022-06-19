using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto.Analyst;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;
        public TicketRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption;
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketByShowtime(int showtimeid)
        {
            if(!await _context.Database.CanConnectAsync())
                return null;
            List<Ticket> list = new List<Ticket>();
            var query = _context.Tickets.Where(a => a.ShowTimeId == showtimeid);
            list = await query.ToListAsync();
            return list;
        }
        public async Task<IEnumerable<Ticket>> GetAllTicketByTransactionId(Guid id)
        {
            if(!await _context.Database.CanConnectAsync())
                return null;
            IEnumerable<Ticket> list = new List<Ticket>();
            var query = _context.Tickets.Where(a => a.TransactionId == id);
            list = await query.ToListAsync();
            return list;
        }
        public async Task<Ticket> GetTicketById(long id)
        {
            if(!await _context.Database.CanConnectAsync())
                return null;
            Ticket ticket = await _context.Tickets.FindAsync(id);
            return ticket;
        }
        public async Task<bool> BuyTicket(Guid transactionId, IEnumerable<long> idList)
        {
            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    " UPDATE Tickets " +
                    " SET TransactionId = @transactionId " +
                    " WHERE Id IN @idList ";
                object param = new { transactionId, idList };

                int count = await con.ExecuteAsync(sql, param);

                return count == idList.Count();
            }
        }
        public async Task<int> CreateTicketAsync(TicketListCreateRequest request)
        {
            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                con.Open();
                var transaction = con.BeginTransaction();

                try
                {
                    

                    string sql =
                        @" INSERT INTO Tickets (SeatId, ShowTimeId, ShowStartTime, OriginalPrice, ReceivePrice, SellingPrice, StatusId) 
                       SELECT @SeatId, @ShowTimeId, @ShowStartTime, @OriginalPrice, @ReceivePrice, (@ReceivePrice + st.BonusPrice), 1 
                       FROM Seats s JOIN SeatTypes st ON s.SeatTypeId = st.Id
                       WHERE s.Id = @SeatId ";

                    object param = new { request.Tickets };

                    int count = await con.ExecuteAsync(sql, param);

                    if (count == request.Tickets.Count())
                    {
                        await transaction.CommitAsync();
                        return count;
                    }
                } 
                catch (DbUpdateException e)
                {
                    await transaction.RollbackAsync();
                    throw e;
                }

                return 0;
            }
        }
        public async Task DeleteExpiredTicket()
        {
            await Task.CompletedTask;
        }

        public async Task<bool> IsTicketInShowtimeValid(long showtimeId, IEnumerable<long> ticketId)
        {
            bool valid = false;

            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    " SELECT COUNT(Id) " +
                    " FROM Tickets " +
                    " WHERE ShowTimeId = @showtimeId AND StartTime > GETUTCDATE() AND Id IN @ticketId AND (LockedTime < @now OR LockedTime IS NULL) AND ";
                object param = new { showtimeId, ticketId, now = DateTime.UtcNow };

                int total = await con.ExecuteScalarAsync<int>(sql, param);

                if (total == ticketId.Count()) return true;
            }

            return valid;
        }

        public async Task<IEnumerable<Ticket>> GetTicketByIdListAsync(IEnumerable<long> idList)
        {
            IEnumerable<Ticket> list = new List<Ticket>();

            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    " SELECT t.Id, t.SellingPrice, s.Id, s.Name, s.RoomId, st.Id, st.Name, st.BonusPrice " +
                    " FROM Tickets t JOIN Seats s ON t.SeatId = s.Id " +
                    "   JOIN SeatTypes st ON s.SeatTypeId = st.Id " +
                    " WHERE t.Id IN @idList ";
                object param = new { idList };

                list = await con.QueryAsync<Ticket, Seat, SeatType, Ticket>(sql, 
                    (t, s, st) => 
                    {
                        s.SeatType = st;
                        t.Seat = s;
                        return t;
                    }, param, splitOn: "Id");
            }

            return list;
        }

        public async Task LockTicketAsync(IEnumerable<long> idList)
        {
            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql = 
                    " UPDATE Tickets " +
                    " SET LockedTime = @time " +
                    " WHERE Id IN @idList ";
                object param = new { idList, time = DateTime.UtcNow.AddMinutes(Constants.LOCK_TICKET_IN_MINUTE) };

                await con.ExecuteAsync(sql, param);
            }
        }

        public async Task<Tuple<decimal, decimal>> GetMinAndMaxPriceFromShowTimeId(long showtimeid)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new Exception("DBCONNECTION");
            var query = _context.Tickets.Where(a => a.ShowTimeId == showtimeid);
            decimal min = 0;
            decimal max = 0;
            foreach(var item in query)
            {
                if(item.SellingPrice > max) max = item.SellingPrice;
                else if(item.SellingPrice < min) min = item.SellingPrice;
            }
            return Tuple.Create(min, max);
        }

        public async Task<TicketQuanityDTO> GetQuickTicketQuanityInTwoWeek(int companyid,
            DateTime ThisMondayWeek, DateTime MondayPreviousWeek, DateTime SundayPreviousWeek)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new Exception("DBCONNECTION");
            //Get every theater id belong in that company
            var showtimeids = await _context.Theaters
                                .Include(a => a.Showtimes)
                                .Where(a => a.CompanyId == 1)
                                .SelectMany(a => a.Showtimes)
                                .Select(a => a.Id).ToListAsync();
            if (!showtimeids.Any())
                return null; //công ty này chưa có cái rạp nào cả...
            //lấy ra số ticket có trong toàn bộ showtimeids có được từ query trên và count lại toàn bộ để ra quantity
            //đây chỉ là tuần hiện tại
            var firstquery = await _context.Showtimes
                                .Include(a => a.Tickets)
                                .Where(a => a.Tickets.Any(
                                    a => showtimeids.Contains(a.ShowTimeId != null ? a.ShowTimeId.Value : -1))
                                    && a.StartTime <= DateTime.UtcNow.Date && a.StartTime >= ThisMondayWeek)
                                .SelectMany(a => a.Tickets)
                                .CountAsync();
            //lấy tuần trước
            var secondquery = await _context.Showtimes
                                .Include(a => a.Tickets)
                                .Where(a => a.Tickets.Any(
                                    a => showtimeids.Contains(a.ShowTimeId != null ? a.ShowTimeId.Value : -1))
                                    && a.StartTime <= SundayPreviousWeek && a.StartTime >= MondayPreviousWeek)
                                .SelectMany(a => a.Tickets)
                                .CountAsync();
            var thirdquery = await _context.Showtimes
                                .Include(a => a.Tickets)
                                .Where(a => a.Tickets.Any
                                (a => showtimeids.Contains(a.ShowTimeId != null ? a.ShowTimeId.Value : -1)))
                                .SelectMany(a => a.Tickets)
                                .CountAsync();
            TicketQuanityDTO dto = new TicketQuanityDTO();
            dto.NewTicketSoldQuantity = firstquery;
            dto.OldTicketSoldQuantity = secondquery;
            dto.TotalTicketSoldQuantity = thirdquery;
            if (firstquery > secondquery)
            {
                dto.IsTicketSoldUpOrDown = true;
                dto.PercentTicketSoldChange = (float)100F - ((float)secondquery * (float)100F / (float)firstquery);
            }
            else if (firstquery < secondquery)
            {
                dto.IsTicketSoldUpOrDown = false;
                dto.PercentTicketSoldChange = (float)100F - ((float)firstquery * (float)100F / (float)secondquery);
            }
            else if( firstquery == secondquery)
            {
                dto.IsTicketSoldUpOrDown = true;
                dto.PercentTicketSoldChange = 0.69F;
            }
            return dto;
        }

    }
}
