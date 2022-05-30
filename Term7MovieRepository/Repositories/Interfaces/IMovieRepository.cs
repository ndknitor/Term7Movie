using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAllMovie();
        Movie GetMovieById(int id);
        int CreateMovie(Movie movie);
        int UpdateMovie(Movie movie);
        int DeleteMovie(int id);
        int Count();
        Task<IEnumerable<Movie>> GetThreeLatestMovie();
    }
}
