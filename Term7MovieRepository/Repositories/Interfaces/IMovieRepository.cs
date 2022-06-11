using Term7MovieCore.Entities;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto.Errors;
using Term7MovieCore.Data.Request.Movie;
using Term7MovieCore.Data.Dto.Movie;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<PagingList<MovieModelDto>> GetAllMovie(ParentFilterRequest request);
        Task<Movie> GetMovieById(int id);
        Task<bool> CreateMovie(IEnumerable<Movie> movie);
        Task<bool> UpdateMovie(Movie movie);
        Task<bool> DeleteMovie(int movieid);
        int Count();
        Task<IEnumerable<Movie>> GetLessThanThreeLosslessLatestMovies();
        Task<IEnumerable<Movie>> GetEightLatestMovies();
        Task<Dictionary<int, IEnumerable<MovieType>>> GetCategoriesFromMovieList(int[] MovieIds);
        Task<IEnumerable<Movie>> GetMoviesFromSpecificPage(int page, int pageCapacity);
        Task<IEnumerable<MovieType>> GetCategoryFromSpecificMovieId(int movieId);
        Task<CreateMovieError> CreateMovieWithCategory(MovieCreateRequest request);
        Task<bool> UpdateMovie(MovieUpdateRequest request);
    }
}
