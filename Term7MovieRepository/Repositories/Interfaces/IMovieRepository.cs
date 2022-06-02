using Term7MovieCore.Entities;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Collections;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMovie();
        Task<Movie> GetMovieById(int id);
        Task CreateMovie(Movie movie);
        Task UpdateMovie(Movie movie);
        Task DeleteMovie(Movie movie);
        int Count();
        Task<IEnumerable<Movie>> GetLessThanThreeLosslessLatestMovies();
        Task<IEnumerable<Movie>> GetEightLatestMovies();
        Task<Dictionary<int, IEnumerable<MovieType>>> GetCategoriesFromMovieList(int[] MovieIds);
        Task<IEnumerable<Movie>> GiveMeEveryMovieYouHave();
        Task<IEnumerable<Movie>> GetMoviesFromSpecificPage(int page, int pageCapacity);
    }
}
