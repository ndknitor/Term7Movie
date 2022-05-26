using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class ShowtimeRepository : IShowtimeRepository
    {
        private AppDbContext _context;
        public ShowtimeRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Showtime> GetAllShowtime(int theaterId)
        {
            IEnumerable<Showtime> list = new List<Showtime>();
            return list;
        }
        public Showtime GetShowtimeById(long id)
        {
            Showtime showtime = null;
            return showtime;
        }
        public int CreateShowtime(Showtime showtime)
        {
            int count = 0;
            return count;
        }
        public int UpdateShowtime(Showtime showtime)
        {
            int count = 0;
            return count;

        }
        public int DeleteShowtimeById(long id)
        {
            int count = 0;
            return count;
        }
    }
}
