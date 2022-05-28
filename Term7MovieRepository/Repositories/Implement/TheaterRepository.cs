using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class TheaterRepository : ITheaterRepository
    {
        private AppDbContext _context;
        public TheaterRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Theater> GetAllTheater()
        {
            IEnumerable<Theater> list = new List<Theater>();
            return list;
        }
        public Theater GetTheaterById(int id)
        {
            Theater theater = null;
            return theater;
        }
        public int CreateTheater(Theater theater)
        {
            int count = 0;
            return count;
        }
        public int UpdateTheater(Theater theater)
        {
            int count = 0;
            return count;
        }
        public int DeleteTheater(int id)
        {
            int count = 0;
            return count;
        }
    }
}
