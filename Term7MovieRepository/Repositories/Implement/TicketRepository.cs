using Microsoft.EntityFrameworkCore;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class TicketRepository : ITicketRepository
    {
        private AppDbContext _context;
        public TicketRepository(AppDbContext context)
        {
            _context = context;
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
            Ticket? ticket = await _context.Tickets.FindAsync(id);
            return ticket;
        }
        public async Task BuyTicket(long transactionId)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
