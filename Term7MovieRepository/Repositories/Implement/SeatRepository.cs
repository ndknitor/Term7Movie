using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Seat>> GetAllSeat(int roomId)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new Exception();
            IEnumerable<Seat> list = new List<Seat>();
            var query = _context.Seats.Where(a => a.RoomId == roomId);
            list = await query.ToListAsync();
            return list;
        }
        public async Task<Seat> GetSeatById(long id)
        {
            Seat seat = await _context.Seats.FindAsync(id);
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
