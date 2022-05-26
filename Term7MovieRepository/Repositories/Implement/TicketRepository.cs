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

        public IEnumerable<Ticket> GetAllTicketByShowtime(Showtime showtime)
        {
            IEnumerable<Ticket> list = new List<Ticket>();
            return list;
        }
        public IEnumerable<Ticket> GetAllTicketByTransactionId(long id)
        {
            IEnumerable<Ticket> list = new List<Ticket>();
            return list;
        }
        public Ticket GetTicketById(long id)
        {
            Ticket ticket = null;
            return ticket;
        }
        public int BuyTicket(long transactionId)
        {
            int count = 0;
            return count;
        }
        public int CreateTicket(Ticket ticket)
        {
            int count = 0;
            return count;
        }
        public int CreateTicket(Ticket[] tickets)
        {
            int count = 0;
            return count;
        }
        public int DeleteExpiredTicket()
        {
            int count = 0;
            return count;
        }
    }
}
