using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IMovieActorRepository
    {
        IEnumerable<MovieActor> GetAllMovieActor(int movieId);
        int CreateMovieActor(MovieActor movieActor);
        int CreateMovieActor(MovieActor[] movieActors);
        int UpdateMovieActor(MovieActor movieActor);
        int DeleteMovieActor(MovieActor movieActor);
    }
}
