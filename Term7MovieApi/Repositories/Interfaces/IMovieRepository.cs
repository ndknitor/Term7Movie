using Term7MovieApi.Entities;

namespace Term7MovieApi.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAllMovie();
        Movie GetMovieById(int id);
        int CreateMovie(Movie movie);
        int UpdateMovie(Movie movie);
        int DeleteMovie(int id);
        int Count();
    }
}
