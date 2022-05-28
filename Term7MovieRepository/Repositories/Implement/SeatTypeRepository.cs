using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class SeatTypeRepository : ISeatTypeRepository
    {
        private AppDbContext _context;
        public SeatTypeRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<SeatType> GetAllSeatType()
        {
            IEnumerable<SeatType> list = new List<SeatType>();
            return list;
        }
    }
}
