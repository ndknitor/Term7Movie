using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class MovieActorRepository : IMovieActorRepository
    {

        private AppDbContext _context;
        public MovieActorRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<MovieActor> GetAllMovieActor(int movieId)
        {
            IEnumerable<MovieActor> list = new List<MovieActor>();
            return list;
        }
        public int CreateMovieActor(MovieActor movieActor)
        {
            int count = 0;
            return count;
        }
        public int CreateMovieActor(MovieActor[] movieActors)
        {
            int count = 0;
            return count;
        }
        public int UpdateMovieActor(MovieActor movieActor)
        {
            int count = 0;
            return count;
        }
        public int DeleteMovieActor(MovieActor movieActor)
        {
            int count = 0;
            return count;
        }
    }
}
