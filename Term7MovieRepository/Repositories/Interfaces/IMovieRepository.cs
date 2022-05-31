using Term7MovieCore.Entities;
using Term7MovieCore.Data.Dto;

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
        Task<IEnumerable<Movie>> GetLessThanThreeLosslessLatestMovies();
        Task<IEnumerable<Movie>> GetEightLatestMovies();
        Task<Dictionary<int, IEnumerable<MovieType>>> GetCategoriesFromMovieList(int[] MovieIds);
    }
}
