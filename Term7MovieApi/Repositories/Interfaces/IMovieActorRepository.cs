using Term7MovieApi.Entities;

namespace Term7MovieApi.Repositories.Interfaces
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
