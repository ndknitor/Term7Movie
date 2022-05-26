using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IMovieCategoryRepository
    {
        IEnumerable<MovieCategory> GetAllMovieCategory(int movieId);
        int CreateMovieCategory(MovieCategory movieCategory);
        int CreateMovieCategory(MovieCategory[] movieCategories);
        int UpdateMovieCategory(MovieCategory movieCategory);
        int DeleteMovieCategory(MovieCategory movieCategory);
    }
}
