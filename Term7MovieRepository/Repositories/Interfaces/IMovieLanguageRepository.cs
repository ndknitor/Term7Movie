using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IMovieLanguageRepository
    {
        IEnumerable<MovieLanguage> GetAllMovieLanguage(int movieId);
        int CreateMovieLanguage(MovieLanguage movieLanguage);
        int CreateMovieLanguage(MovieLanguage[] movieLanguages);
        int UpdateMovieLanguage(MovieLanguage movieLanguage);
        int DeleteMovieLanguage(MovieLanguage movieLanguage);
    }
}
