using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class SeatRepository : ISeatRepository
    {
        private AppDbContext _context;
        public SeatRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Seat> GetAllSeat(int roomId)
        {
            IEnumerable<Seat> list = new List<Seat>();
            return list;
        }
        public Seat GetSeatById(long id)
        {
            Seat seat = null;
            return seat;
        }
        public int CreateSeat(Seat seat)
        {
            int count = 0;
            return count;
        }
        public int CreateSeat(Seat[] seat)
        {
            int count = 0;
            return count;
        }
        public int UpdateSeat(Seat seat)
        {
            int count = 0;
            return count;
        }
        public int DeleteSeat(long id)
        {
            int count = 0;
            return count;
        }
    }
}
