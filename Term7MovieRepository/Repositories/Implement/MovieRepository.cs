using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;
namespace Term7MovieRepository.Repositories.Implement
{
    public class MovieRepository : IMovieRepository
    {

        private AppDbContext _context;
        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Movie> GetAllMovie()
        {
            IEnumerable<Movie> list = new List<Movie>();
            return list;
        }
        public Movie GetMovieById(int id)
        {
            Movie movie = null;
            return movie;
        }
        public int CreateMovie(Movie movie)
        {
            int count = 0;
            return count;
        }
        public int UpdateMovie(Movie movie)
        {
            int count = 0;
            return count;
        }
        public int DeleteMovie(int id)
        {
            int count = 0;
            return count;
        }
        public int Count()
        {
            int count = 0;
            return count;
        }
    }
}
