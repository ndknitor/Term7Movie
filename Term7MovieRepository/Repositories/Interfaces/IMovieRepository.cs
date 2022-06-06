using Term7MovieCore.Entities;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Collections;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<PagingList<MovieModelDto>> GetAllMovie(ParentFilterRequest request);
        Task<Movie> GetMovieById(int id);
        Task<bool> CreateMovie(IEnumerable<Movie> movie);
        Task<bool> UpdateMovie(Movie movie);
        Task<bool> DeleteMovie(Movie movie);
        int Count();
        Task<IEnumerable<Movie>> GetLessThanThreeLosslessLatestMovies();
        Task<IEnumerable<Movie>> GetEightLatestMovies();
        Task<Dictionary<int, IEnumerable<MovieType>>> GetCategoriesFromMovieList(int[] MovieIds);
        Task<IEnumerable<Movie>> GetMoviesFromSpecificPage(int page, int pageCapacity);
        Task<IEnumerable<MovieType>> GetCategoryFromSpecificMovieId(int movieId);
    }
}
