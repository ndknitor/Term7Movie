using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class MovieLanguageRepository : IMovieLanguageRepository
    {

        private AppDbContext _context;
        public MovieLanguageRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<MovieLanguage> GetAllMovieLanguage(int movieId)
        {
            IEnumerable<MovieLanguage> list = new List<MovieLanguage>();
            return list;
        }
        public int CreateMovieLanguage(MovieLanguage movieLanguage)
        {
            int count = 0;
            return count;
        }
        public int CreateMovieLanguage(MovieLanguage[] movieLanguages)
        {
            int count = 0;
            return count;
        }
        public int UpdateMovieLanguage(MovieLanguage movieLanguage)
        {
            int count = 0;
            return count;
        }
        public int DeleteMovieLanguage(MovieLanguage movieLanguage)
        {
            int count = 0;
            return count;
        }
    }
}
