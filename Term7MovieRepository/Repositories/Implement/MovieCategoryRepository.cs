using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class MovieCategoryRepository : IMovieCategoryRepository
    {
        private AppDbContext _context;
        public MovieCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MovieCategory> GetAllMovieCategory(int movieId)
        {
            IEnumerable<MovieCategory> list = new List<MovieCategory>();
            return list;
        }
        public int CreateMovieCategory(MovieCategory movieCategory)
        {
            int count = 0;
            return count;
        }
        public int CreateMovieCategory(MovieCategory[] movieCategories)
        {
            int count = 0;
            return count;
        }
        public int UpdateMovieCategory(MovieCategory movieCategory)
        {
            int count = 0;
            return count;
        }
        public int DeleteMovieCategory(MovieCategory movieCategory)
        {
            int count = 0;
            return count;
        }
    }
}
