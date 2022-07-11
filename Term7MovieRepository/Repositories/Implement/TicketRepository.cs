using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Dto.Analyst;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;
using Term7MovieRepository.Cache.Interface;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;
        private readonly ProfitFormulaOption _profitFormulaOption;
        private readonly ICacheProvider _cacheProvider;

        private const string FILTER_BY_ID = "Id";
        private const string FILTER_BY_SHOWTIME = "ShowtimeId";
        private const string FILTER_BY_IS_SHOWED = "IsShowed";
        private const string FILTER_BY_IS_PURCHASED = "IsPurchased";

        public TicketRepository(AppDbContext context, ConnectionOption connectionOption, 
                                ProfitFormulaOption profitFormulaOption, ICacheProvider cacheProvider)
        {
            _context = context;
            _connectionOption = connectionOption;
            _profitFormulaOption = profitFormulaOption;
            _cacheProvider = cacheProvider;
        }

        public async Task<PagingList<TicketDto>> GetAllTicketAsync(TicketFilterRequest request)
        {

            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int offset = request.PageSize * (request.Page - 1);
                int fetch = request.PageSize;
                string sql =
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
                       WHERE 1 = 1 " + 

                            GetAdditionalTicketFilter(request, FILTER_BY_SHOWTIME) +
                            GetAdditionalTicketFilter(request, FILTER_BY_ID) +
                            GetAdditionalTicketFilter(request, FILTER_BY_IS_SHOWED) +
                            GetAdditionalTicketFilter(request, FILTER_BY_IS_PURCHASED) +

                    @" ORDER BY t.Id DESC 
                       OFFSET @offset ROWS
                       FETCH NEXT @fetch ROWS ONLY ;";

                string count =
                    @" SELECT COUNT(*) 
                       FROM Tickets t 
                       WHERE 1 = 1 " +
                       GetAdditionalTicketFilter(request, FILTER_BY_SHOWTIME) +
                       GetAdditionalTicketFilter(request, FILTER_BY_ID) +
                       GetAdditionalTicketFilter(request, FILTER_BY_IS_SHOWED) +
                       GetAdditionalTicketFilter(request, FILTER_BY_IS_PURCHASED);


                object param = new { offset, fetch, request.ShowtimeId, request.TicketId, request.IsNotShowedYet, request.IsPurchased };

                var multiQ = await con.QueryMultipleAsync(sql + count, param);

                IEnumerable<TicketDto> tickets = multiQ.Read<TicketDto, SeatDto, SeatTypeDto, TicketTypeDto, TicketDto>(
                    (t, s, st, tt) =>
                    {
                        t.TicketType = tt;
                        s.SeatType = st;
                        t.Seat = s;
                        return t;
                    }, splitOn: "Id"
                );

                long total = await multiQ.ReadFirstOrDefaultAsync<long>();

                return new PagingList<TicketDto>(request.PageSize, request.Page, tickets, total);

            }
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
        public async Task<TicketDto> GetTicketById(long id, bool isNotShowed)
        {
            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    @" SELECT t.Id, t.ShowtimeId, t.ShowStartTime, 
                              t.ReceivePrice, t.SellingPrice, t.StatusId, ts.Name 'StatusName',
                              t.LockedTime, t.TransactionId, t.ShowtimeTicketTypeId,
                              t.SeatId, s.Id, s.Name, s.RoomId, s.ColumnPos, s.RowPos, s.SeatTypeId, st.Id, st.Name, 
                              tt.Id, tt.Name, tt.CompanyId,   
                              sh.Id, sh.MovieId, sh.RoomId, sh.StartTime, sh.EndTime, sh.TheaterId, th.Name 'TheaterName', m.Title 'MovieTitle'
                       FROM Tickets t JOIN Seats s ON t.SeatId = s.Id
                            JOIN SeatTypes st ON s.SeatTypeId = st.Id
                            JOIN TicketStatuses ts ON t.StatusId = ts.Id 
                            JOIN ShowtimeTicketTypes shtt ON shtt.Id = t.ShowtimeTicketTypeId 
                            JOIN TicketTypes tt ON shtt.TicketTypeId = tt.Id
                            JOIN Showtimes sh ON shtt.ShowtimeId = sh.Id
                            JOIN Movies m ON m.Id = sh.MovieId
                            JOIN Theaters th ON sh.TheaterId = th.Id 
                       WHERE t.Id = @id " +

                       GetAdditionalTicketFilter( new TicketFilterRequest { IsNotShowedYet = isNotShowed }, FILTER_BY_IS_SHOWED);
                object param = new { id };

                IEnumerable<TicketDto> tickets = await con.QueryAsync<TicketDto, SeatDto, SeatTypeDto, TicketTypeDto, ShowtimeDto, TicketDto>( sql,
                    (t, s, st, tt, sh) =>
                    {
                        t.TicketType = tt;
                        s.SeatType = st;
                        t.Seat = s;
                        t.Showtime = sh;
                        return t;
                    }, param, splitOn: "Id"
                );

                return tickets.FirstOrDefault();
            }
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
                int count = 0;
                try
                {
                    foreach(TicketCreateRequest ticket in request.Tickets)
                    {
                        string sql =
                        @" INSERT INTO Tickets (SeatId, ShowTimeId, ShowStartTime, ReceivePrice, SellingPrice, StatusId, ShowtimeTicketTypeId ) 
                           SELECT @SeatId, sh.Id, sh.StartTime , shtt.ReceivePrice, ROUND(shtt.ReceivePrice*(1 + @SellingPriceRatio), 0), 1, @ShowtimeTicketTypeId
                           FROM ShowtimeTicketTypes shtt 
                                JOIN Showtimes sh ON shtt.ShowtimeId = sh.Id
                                JOIN Seats s ON s.RoomId = sh.RoomId
                           WHERE s.Id = @SeatId 
                                AND shtt.Id = @ShowtimeTicketTypeId ";

                        object param = new 
                        { 
                            ticket.SeatId,
                            ticket.ShowtimeTicketTypeId,
                            _profitFormulaOption.SellingPriceRatio };

                        count += await con.ExecuteAsync(sql, param, transaction: transaction);
                    }

                    if (count == request.Tickets.Count())
                    {
                        await transaction.CommitAsync();

                        ShowtimeDto showtime = GetShowtimeByShowtimeTicketType(request.Tickets.First().ShowtimeTicketTypeId);

                        IEnumerable<TicketDto> tickets = await GetTicketByShowtimeId(showtime.Id);

                        await _cacheProvider.PutHashMapAsync(Constants.REDIS_KEY_SHOWTIME_TICKET, tickets);

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

        private async Task<IEnumerable<TicketDto>> GetTicketByShowtimeId(long showtimeId)
        {
            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                await con.OpenAsync();
                string sql =
                    @" SELECT t.Id, t.ShowtimeId, t.ShowStartTime, 
                              t.ReceivePrice, t.SellingPrice, t.StatusId, ts.Name 'StatusName',
                              t.LockedTime, t.TransactionId, t.ShowtimeTicketTypeId,
                              t.SeatId, s.Id, s.Name, s.ColumnPos, s.RowPos, s.SeatTypeId, st.Id, st.Name,  
                              tt.Id, tt.Name, tt.CompanyId,
                              sh.Id, sh.MovieId, sh.RoomId, sh.StartTime, sh.EndTime, sh.TheaterId, th.Name 'TheaterName', m.Title 'MovieTitle'
                       FROM Tickets t JOIN Seats s ON t.SeatId = s.Id
                            JOIN SeatTypes st ON s.SeatTypeId = st.Id
                            JOIN TicketStatuses ts ON t.StatusId = ts.Id 
                            JOIN ShowtimeTicketTypes shtt ON shtt.Id = t.ShowtimeTicketTypeId 
                            JOIN TicketTypes tt ON shtt.TicketTypeId = tt.Id
                            JOIN Showtimes sh ON shtt.ShowtimeId = sh.Id
                            JOIN Movies m ON m.Id = sh.MovieId
                            JOIN Theaters th ON sh.TheaterId = th.Id 
                       WHERE t.ShowtimeId = @showtimeId ";


                object param = new { showtimeId };

                IEnumerable<TicketDto> tickets = await con.QueryAsync<TicketDto, SeatDto, SeatTypeDto, TicketTypeDto, ShowtimeDto,TicketDto>(sql,
                    (t, s, st, tt, sh) =>
                    {
                        t.TicketType = tt;
                        s.SeatType = st;
                        t.Seat = s;
                        t.Showtime = sh;
                        return t;
                    }, param, splitOn: "Id"
                );

                return tickets;
            }
        }

        private ShowtimeDto GetShowtimeByShowtimeTicketType(Guid showtimeTicketTypeId)
        {
            ShowtimeDto showtimeDto = null;

            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql = @" SELECT sh.Id, sh.StartTime
                                FROM Showtimes sh JOIN ShowtimeTicketTypes shtt ON sh.Id = shtt.ShowtimeId 
                                WHERE shtt.Id = @showtimeTicketTypeId ";
                object param = new { showtimeTicketTypeId};
                showtimeDto = con.QueryFirstOrDefault<ShowtimeDto>(sql, param);
            }


            return showtimeDto;
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

        public IEnumerable<TicketDto> GetTicketByIdList(IEnumerable<long> idList)
        {
            IEnumerable<TicketDto> list = null;

            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    @" SELECT t.Id, t.SellingPrice, tt.Id, tt.Name 
                       FROM Tickets t JOIN ShowtimeTicketTypes shtt ON t.ShowtimeTicketTypeId = shtt.Id
                            JOIN TicketTypes tt ON shtt.TicketTypeId = tt.Id 
                       WHERE t.Id IN @idList 
                            AND t.ShowStartTime > GETUTCDATE() 
                            AND ( t.LockedTime IS NULL OR t.LockedTime < GETUTCDATE()) 
                            AND t.TransactionId IS NULL ";

                object param = new { idList };

                list = con.Query<TicketDto, TicketTypeDto, TicketDto>(sql, 
                    (t, tt) =>
                    {
                        t.TicketType = tt;
                        return t;
                    }, param, splitOn: "Id");
            }

            return list;
        }

        public void LockTicket(IEnumerable<long> idList)
        {
            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql = 
                    " UPDATE Tickets " +
                    " SET LockedTime = @time " +
                    " WHERE Id IN @idList ";
                object param = new { idList, time = DateTime.UtcNow.AddMinutes(Constants.LOCK_TICKET_IN_MINUTE) };

                con.Execute(sql, param);
            }
        }

        public async Task<int> LockTicketAsync(long ticketId)
        {
            int count = 0;
            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    " UPDATE Tickets " +
                    " SET LockedTime = @time " +
                    " WHERE Id = @id ";
                object param = new { ticketId, time = DateTime.UtcNow.AddMinutes(Constants.LOCK_TICKET_IN_MINUTE) };

                count = await con.ExecuteAsync(sql, param);
            }
            return count;
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
                                .Where(a => a.CompanyId == companyid)
                                .SelectMany(a => a.Showtimes)
                                .Select(a => a.Id).ToListAsync();
            //lấy ra số ticket có trong toàn bộ showtimeids có được từ query trên và count lại toàn bộ để ra quantity
            //đây chỉ là tuần hiện tại
            var firstquery = await _context.Showtimes
                                .Where(a => showtimeids.Contains(a.Id)
                                    && a.StartTime <= DateTime.UtcNow.Date && a.StartTime >= ThisMondayWeek)
                                .CountAsync();
            //lấy tuần trước
            var secondquery = await _context.Showtimes
                                .Where(a => showtimeids.Contains(a.Id)
                                    && a.StartTime <= SundayPreviousWeek && a.StartTime >= MondayPreviousWeek)
                                .CountAsync();
            var thirdquery = await _context.Showtimes
                                .Where(a => showtimeids.Contains(a.Id))
                                .CountAsync();
            if (firstquery == 0 && secondquery == 0 && thirdquery == 0)
            {
                firstquery = 1;
                secondquery = 1;
                thirdquery = 1;
            }
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
        private string GetAdditionalTicketFilter(TicketFilterRequest request, string filter)
        {
            string sql = "";
            switch(filter)
            {
                case FILTER_BY_SHOWTIME:

                    if (request.ShowtimeId != null) sql = " AND t.ShowtimeId = @ShowtimeId ";

                    break;
                case FILTER_BY_ID:

                    if (request.TicketId != null) sql = " AND t.Id = @TicketId ";

                    break;
                case FILTER_BY_IS_PURCHASED:

                    if (request.IsPurchased) sql = " AND t.TransactionId IS NOT NULL ";

                    break;
                case FILTER_BY_IS_SHOWED:

                    if (request.IsNotShowedYet) sql = " AND t.ShowStartTime > GETUTCDATE() ";

                    break;
            }
            return sql;
        }
    }
}
