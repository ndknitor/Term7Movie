using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Options;
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
        public async Task CreateTicket(Ticket ticket)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new Exception("DBCONNECT");
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }
        public async Task CreateTicket(IEnumerable<Ticket> tickets)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new Exception("DBCONNECT");
            await _context.Tickets.AddRangeAsync(tickets);
            await _context.SaveChangesAsync();
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
    }
}
