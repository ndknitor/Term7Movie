using Term7MovieCore.Entities;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Collections;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<PagingList<MovieDto>> GetAllMovie(ParentFilterRequest request);
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
