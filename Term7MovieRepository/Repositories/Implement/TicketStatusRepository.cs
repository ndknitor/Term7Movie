using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class TicketStatusRepository : ITicketStatusRepository
    {
        private AppDbContext _context;
        public TicketStatusRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<TicketStatus> GetAllTicketStatus()
        {
            IEnumerable<TicketStatus> list = new List<TicketStatus>();
            return list;
        }
    }
}
